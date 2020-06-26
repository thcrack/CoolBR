function Main(p, dataRef, actionRef){
    const data = dataRef.current;
    let containerDiv;
    p.setup = () => {
        containerDiv = p.select('#target-canvas-container');
        p.createCanvas(containerDiv.size().width, 400);
        p.background(0);
        p.stroke(120);
    };

    p.draw = () => {
        if(data.grid.rows === 0 || data.grid.cols === 0) return;
        let w = p.width / data.grid.cols, h = p.height / data.grid.rows;
        for(let i = 0; i < data.grid.rows; i++){
            for(let j = 0; j < data.grid.cols; j++){
                p.fill(data.grid.data[i * data.grid.cols + j] === 1 ? '#ffffff' : '#000000');
                p.rect(w * j, h * i, w, h);
            }
        }
    };
    
    p.windowResized = () => {
        let newSize = containerDiv.size();
        p.resizeCanvas(newSize.width, 400);
    }
    
    p.mouseClicked = (e) => {
        if(isOutsideCanvas(p.mouseX, p.mouseY)) return;
        let col = Math.floor(p.mouseX / p.width * data.grid.cols);
        let row = Math.floor(p.mouseY / p.height * data.grid.rows);
        data.grid.data[row * data.grid.cols + col] ^= 1;
        actionRef.current.updateGrid(row, col);
    }
    
    const isOutsideCanvas = (x, y) => {
        return x < 0 || x >= p.width
            || y < 0 || y >= p.height;
    }
    
    p.keyPressed = (e) => {
    }
}

export default Main;