import React, { useState, useEffect, useRef } from 'react';
import * as p5 from 'p5';
import Main from '../game/Main'
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

function GameCanvas(props){
    const [ready, setReady] = useState(false);
    const dataRef = useRef({
        grid: {rows: 0, cols: 0, data: []}
    });
    const connectionRef = useRef();
    
    useEffect(() => {
        const populateGrid = async () => {
            const response = await fetch('game');
            dataRef.current.grid = await response.json();
        }
        populateGrid();
        connectionRef.current = new SignalR.HubConnectionBuilder()
           .withUrl("/hub/game").build();
        connectionRef.current.on("GridChanged",
            (row, col, id) => {
                dataRef.current.grid.data[row * dataRef.current.grid.cols + col] = id;
            });
        connectionRef.current.start();
    });
    
    // Can be used to provide p5.js APIs to callback
    const actionRef = useRef({
        updateGrid: async (row, col) => {
            await fetch('game/set?row=' + row + '&col=' + col);
        }
    });

    const p5Function = (p5Ref) => {
        return new p5(p => Main(p, dataRef, actionRef), p5Ref);
    }
    
    return (
        <div id="target-canvas-container">
            <div className="target-canvas">
                {useDOMControl(p5Function)}
            </div>
        </div>
    );
}

export default GameCanvas;