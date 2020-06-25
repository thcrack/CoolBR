function Main(p, dataRef, actionRef){
    const data = dataRef.current;
    let containerDiv;
    let currPos = {x: 0, y: 0};
    let colors = ['#111d5e', '#c70039', '#f37121', '#ffbd69'];
    p.setup = () => {
        containerDiv = p.select('#target-canvas-container');
        p.createCanvas(containerDiv.size().width, 400);
        p.background(0);
        p.noStroke();
    };

    p.draw = () => {
        currPos.x = p.lerp(currPos.x, data.pos.x, 0.1);
        currPos.y = p.lerp(currPos.y, data.pos.y, 0.1);
        p.background(0, 10);
        let quarterWidth = p.width / 4;
        for(let i = 0; i < 4; i++){
            p.fill(colors[i]);
            p.rect(quarterWidth * i, 0, quarterWidth, p.height);
        }
        p.fill(255);
        p.ellipse(currPos.x, currPos.y, 50, 50);
    };
    
    p.windowResized = () => {
        let newSize = containerDiv.size();
        p.resizeCanvas(newSize.width, 400);
    }
    
    p.mouseClicked = (e) => {
        if(p.mouseX < 0 || p.mouseX > p.width
        || p.mouseY < 0 || p.mouseY > p.height) {
            p.select('body').style('background-color', '#FFFFFF');
        }else{
            p.select('body').style('background-color', colors[Math.floor(p.mouseX * 4 / (p.width + 1))]);
        }
    }
    
    p.keyPressed = (e) => {
    }
}

export default Main;