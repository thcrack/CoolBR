function P5CoolBR(p, dataRef, actionRef){
    const data = dataRef.current;
    const actions = actionRef.current;
    let containerDiv;
    let isFocused = false;
    let gridSize = 64;
    let camSpeedMultiplier = 5;
    let camMoveMargin = .1;
    let renderer;
    let tilesetImg;
    p.setup = () => {
        containerDiv = p.select('#target-canvas-container');
        renderer = p.createCanvas(containerDiv.size().width, 512, p.WEBGL);
        p.glMagFilter = p.drawingContext.NEAREST;
        p.glMinFilter = p.drawingContext.NEAREST;
        p.background(0);
        tilesetImg = p.loadImage("/gameassets/monochrome_transparent_packed.png");
        let tex = renderer.getTexture(tilesetImg);
        tex.setInterpolation(p.NEAREST, p.NEAREST);
    };
    
    let targetPos = {x: 0, y: 0};
    let currPos = {x: 0, y: 0};

    p.draw = () => {
        if(isFocused){
            let camXmove = 0, camYmove = 0;
            if(p.mouseX < p.width * camMoveMargin) camXmove = -1;
            else if(p.mouseX > p.width * (1 - camMoveMargin)) camXmove = 1;
            if(p.mouseY < p.height * camMoveMargin) camYmove = -1;
            else if(p.mouseY > p.height * (1 - camMoveMargin)) camYmove = 1;
            targetPos.x += camXmove * camSpeedMultiplier;
            targetPos.y += camYmove * camSpeedMultiplier;
            targetPos.x = Math.max(0, Math.min(gridSize * data.grid.cols, targetPos.x));
            targetPos.y = Math.max(0, Math.min(gridSize * data.grid.rows, targetPos.y));
        }
        currPos.x = p.lerp(currPos.x, targetPos.x, 0.1);
        currPos.y = p.lerp(currPos.y, targetPos.y, 0.1);
        p.push();
        p.translate(-currPos.x, -currPos.y);
        p.background('#3e1f03');
        let rowMin = Math.max(0, Math.floor((currPos.y - p.height / 2) / gridSize - 1));
        let colMin = Math.max(0, Math.floor((currPos.x - p.width / 2) / gridSize - 1));
        let rowMax = Math.min(data.grid.rows, Math.ceil((p.height / 2 + currPos.y) / gridSize));
        let colMax = Math.min(data.grid.cols, Math.ceil((p.width / 2 + currPos.x) / gridSize));
        for(let i = rowMin; i < rowMax; i++){
            for (let j = colMin; j < colMax; j++){
                let id = data.grid.gids[i * data.grid.cols + j] - 1;
                if(id >= 0) {
                    p.image(tilesetImg, j * gridSize, i * gridSize, gridSize, gridSize, 16 * (id % 48), 16 * Math.floor(id / 48), 16, 16);
                }
                if(data.visible[i * data.grid.cols + j] === 0){
                    p.noStroke();
                    p.fill(0, 192);
                    p.rect(j * gridSize, i * gridSize, gridSize, gridSize);
                }
                if(i === data.origin.row && j === data.origin.col){
                    p.noFill();
                    p.stroke(255, 0, 0);
                    p.strokeWeight(4);
                    p.rect(j * gridSize, i * gridSize, gridSize, gridSize);
                }
            }
        }
        p.pop();
    };

    p.windowResized = () => {
        let newSize = containerDiv.size();
        p.resizeCanvas(newSize.width, 400);
    }

    p.mousePressed = (e) => {
        isFocused = !isOutsideCanvas(p.mouseX, p.mouseY);
    }

    p.mouseReleased = (e) => {
        if(isOutsideCanvas(p.mouseX, p.mouseY)) return;
    }
    
    p.mouseWheel = (e) => {
        if(!isFocused) return;
        //move the square according to the vertical scroll amount
        let newGridSize = Math.max(16, Math.min(64, gridSize - e.delta));
        let ratio = newGridSize / gridSize;
        targetPos.x *= ratio;
        currPos.x *= ratio;
        targetPos.y *= ratio;
        currPos.y *= ratio;
        gridSize = newGridSize;
        //uncomment to block page scrolling
        return false;
    }
    
    p.keyPressed = (e) => {
        if(!isFocused) return true;
        let oldR = data.origin.row, oldC = data.origin.col;
        switch (p.keyCode) {
            case p.UP_ARROW:
                data.origin.row = Math.max(0, data.origin.row - 1);
                break;
            case p.DOWN_ARROW:
                data.origin.row = Math.min(data.grid.rows - 1, data.origin.row + 1);
                break;
            case p.RIGHT_ARROW:
                data.origin.col = Math.min(data.grid.cols - 1, data.origin.col + 1);
                break;
            case p.LEFT_ARROW:
                data.origin.col = Math.max(0, data.origin.col - 1);
                break;
            default:
                break;
        }
        if(data.origin.row !== oldR || data.origin.col !== oldC){
            actions.originMoved();
            targetPos = {x: data.origin.col * gridSize, y: data.origin.row * gridSize};
        }
        
        return false;
    }

    const isOutsideCanvas = (x, y) => {
        return x < 0 || x >= p.width
            || y < 0 || y >= p.height;
    }
}

export default P5CoolBR;