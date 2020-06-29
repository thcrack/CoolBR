function Painter(p, dataRef, actionRef){
    const data = dataRef.current;
    let containerDiv;
    const colors = ['#000000', '#888888', '#ffffff'];
    let isFocused = false;
    let canvasDom;
    p.setup = () => {
        containerDiv = p.select('#target-canvas-container');
        canvasDom = p.createCanvas(containerDiv.size().width, 400);
        p.background(0);
        p.stroke(120);
    };

    p.draw = () => {
        if(data.grid.rows === 0 || data.grid.cols === 0) return;
        let w = p.width / data.grid.cols, h = p.height / data.grid.rows;
        for(let i = 0; i < data.grid.rows; i++){
            for(let j = 0; j < data.grid.cols; j++){
                p.fill(colors[data.grid.data[i * data.grid.cols + j]]);
                p.rect(w * j, h * i, w, h);
            }
        }
    };
    
    p.windowResized = () => {
        let newSize = containerDiv.size();
        p.resizeCanvas(newSize.width, 400);
    }
    
    p.mousePressed = (e) => {
        isFocused = isOutsideCanvas(p.mouseX, p.mouseY);
    }
    
    p.mouseReleased = (e) => {
        if(!data.connected || isOutsideCanvas(p.mouseX, p.mouseY)) return;
        let col = Math.floor(p.mouseX / p.width * data.grid.cols);
        let row = Math.floor(p.mouseY / p.height * data.grid.rows);
        let idx = row * data.grid.cols + col;
        data.grid.data[idx] += (p.mouseButton === p.LEFT) ? 1 : 2;
        data.grid.data[idx] %= 3;
        actionRef.current.updateGrid(row, col, data.grid.data[idx]);
    }
    
    const isOutsideCanvas = (x, y) => {
        return x < 0 || x >= p.width
            || y < 0 || y >= p.height;
    }
    
    p.keyPressed = (e) => {
        if(!isFocused) return;
        p.saveCanvas(canvasDom, "Canvas", "png");
    }
}

export default Painter;