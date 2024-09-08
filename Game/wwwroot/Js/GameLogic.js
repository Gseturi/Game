window.initGame = (instance) => {


  
    window.game = instance;
    window.requestAnimationFrame(gameLoop);
};


function gameLoop(timeStamp) {
    window.requestAnimationFrame(gameLoop);

    game.instance.invokeMethodAsync('GameLoop', timeStamp);


};
