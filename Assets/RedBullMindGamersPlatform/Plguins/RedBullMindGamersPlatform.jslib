mergeInto(LibraryManager.library, {

  GameOver: function (level, score) {
    var msg = "gameOver:" + level + ":" + score;
    console.log("postMessage: " + msg);
    parent.postMessage(msg, "*");
  },

  IsMobile: function () {
    return UnityLoader.SystemInfo.mobile;
  },

});