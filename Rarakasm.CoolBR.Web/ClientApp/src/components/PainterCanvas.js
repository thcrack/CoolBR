import React, { useState, useEffect, useRef } from 'react';
import * as p5 from 'p5';
import Painter from '../p5painter/Painter'
import * as SignalR from "@microsoft/signalr";

const useDOMControl = (domFunc) => {
    const domRef = useRef();

    useEffect(() => {
        let canvas = domFunc(domRef.current);
        return () => {
            canvas.remove();
        }
    });
    
    const contextMenu = (e) => {
        e.preventDefault();
    };

    return (
        <div ref={domRef} onContextMenu={contextMenu}>
        </div>
    );
}

function PainterCanvas(props){
    const dataRef = useRef({
        grid: {rows: 0, cols: 0, data: []},
        connected: false
    });
    const connectionRef = useRef();
    
    useEffect(() => {
        const populateGrid = async () => {
            const response = await fetch('api/painter');
            dataRef.current.grid = await response.json();
            dataRef.current.connected = true;
        }
        connectionRef.current = new SignalR.HubConnectionBuilder()
           .withUrl("/hub/painter").build();
        connectionRef.current.on("GridChanged",
            (row, col, id) => {
                dataRef.current.grid.data[row * dataRef.current.grid.cols + col] = id;
            });
        connectionRef.current.start().then(populateGrid);
    });
    
    // Can be used to provide p5.js APIs to callback
    const actionRef = useRef({
        updateGrid: async (row, col, num) => {
            await fetch('api/painter/set?row=' + row
                + '&col=' + col
                + '&num=' + num, { method: 'PUT' });
        }
    });

    const p5Function = (p5Ref) => {
        return new p5(p => Painter(p, dataRef, actionRef), p5Ref);
    }
    
    return (
        <div id="target-canvas-container">
            <div className="target-canvas">
                {useDOMControl(p5Function)}
            </div>
        </div>
    );
}

export default PainterCanvas;