import React, { useState, useEffect, useRef } from 'react';
import * as p5 from 'p5';
import Main from '../game/Main'

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
    
    const [testNum, setTestNum] = useState(0);
    const dataRef = useRef();
    
    useEffect(() => {
       dataRef.current = data; 
    });
    
    let scale = 1;
    
    const actionRef = useRef({
        testNum: (x, y) =>
            {
                setTestNum(x * y);
            },
        alert: () => alert(" ")
    });

    let data = {
        pos: {'x': 150, 'y': 60}
    };

    const p5Function = (p5Ref) => {
        return new p5(p => Main(p, dataRef, actionRef), p5Ref);
    }

    const randomizePosition = function () {
        data.pos = {'x': Math.random() * 800, 'y': Math.random() * 400};
        dataRef.current.pos = data.pos;
        scale = Math.random() * 100;
    }
    
    return (
        <div id="target-canvas-container">
            <div className="target-canvas">
                {useDOMControl(p5Function)}
            </div>
            <button className="btn btn-primary" onClick={randomizePosition}>
                Randomize
            </button>
            <p>{testNum}</p>
        </div>
    );
}

export default GameCanvas;