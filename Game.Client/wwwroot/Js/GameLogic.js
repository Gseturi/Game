function initGame(dotNetHelper) {
    function gameLoop(timestamp) {
        dotNetHelper.invokeMethodAsync('GameLoop', timestamp);
        requestAnimationFrame(gameLoop);
    }

    requestAnimationFrame(gameLoop);
}