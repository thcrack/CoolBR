import React, { useState, useEffect, useRef } from 'react';
import * as p5 from 'p5';
import * as SignalR from "@microsoft/signalr";
import P5CoolBR from "../game/P5CoolBR";

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
    const dataRef = useRef({
        grid: {rows: 0, cols: 0, gids: []},
        origin: {row: 9, col: 15},
        visible: [],
        connected: false
    });
    //const connectionRef = useRef();
    
    const refreshVisible = async () => {
        const visResponse = await fetch(
            'api/game/visiblegrids?row=' + dataRef.current.origin.row
            + '&col=' + dataRef.current.origin.col
            + '&maxRange=' + 6
        );
        let visibles = await visResponse.json();
        dataRef.current.visible.fill(0);
        for(let i = 0; i < visibles.length; i += 2){
            dataRef.current.visible[
            visibles[i] * dataRef.current.grid.cols + visibles[i + 1]
                ] = 1;
        }
    }

    useEffect(() => {
        const populateGrid = async () => {
            const response = await fetch('api/game/mapgids');
            dataRef.current.grid = await response.json();
            dataRef.current.visible
                = new Array(dataRef.current.grid.cols * dataRef.current.grid.rows);
            await refreshVisible();
            dataRef.current.connected = true;
        }
        // connectionRef.current = new SignalR.HubConnectionBuilder()
        //     .withUrl("/hub/painter").build();
        // connectionRef.current.on("GridChanged",
        //     (row, col, id) => {
        //         dataRef.current.grid.data[row * dataRef.current.grid.cols + col] = id;
        //     });
        // connectionRef.current.start().then(populateGrid);
        populateGrid();
    });

    // Can be used to provide p5.js APIs to callback
    const actionRef = useRef({
        originMoved: () => refreshVisible()
    });

    const p5Function = (p5Ref) => {
        return new p5(p => P5CoolBR(p, dataRef, actionRef), p5Ref);
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