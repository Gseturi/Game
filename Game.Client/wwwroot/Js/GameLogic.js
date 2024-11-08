window.initGame = (instance) => {

    var canvasContainer = document.getElementById('canvasContainer'),
        canvases = canvasContainer.getElementsByTagName('canvas') || [];
    window.game = {
        instance: instance,
        canvas: canvases.length ? canvases[0] : null
    };
    console.log("machikela");


    window.addEventListener("resize", onResize);
    onResize();

    window.game.canvas.setAttribute("tabindex", "0"); // Make sure the canvas is focusable
    window.game.canvas.focus();


    if (window.game.canvas) {
        console.log("shevida");
        window.game.canvas.addEventListener('keydown', (e) => {
            console.log("keydown");
            game.instance.invokeMethodAsync("KeyDown", e.keyCode); // Or use e.key
        });

        window.game.canvas.addEventListener("keyup", (e) => {
            const keyCode = e.keyCode;
            console.log("KeyUp event: ", keyCode);

            // Check if keyCode is valid
            
         game.instance.invokeMethodAsync('KeyUp', keyCode);
           
        });
    } else {
        console.error("Canvas not found.");
    }




    window.requestAnimationFrame(gameLoop);


};

function onResize() {
    if (!window.game.canvas)
        return;

    game.canvas.width = window.innerWidth;
    game.canvas.height = window.innerHeight;
}


function gameLoop(timeStamp) {
    window.requestAnimationFrame(gameLoop);
    game.instance.invokeMethodAsync('GameLoop', timeStamp, game.canvas.width, game.canvas.height)
        .catch(error => console.error("Error invoking GameLoop:", error));

};
