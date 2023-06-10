mergeInto(LibraryManager.library, {

  GetUserId: function () {
      var urlParams = new URLSearchParams(window.location.search);
      var id = urlParams.get("uid");
      return id ? id : 0;
  },

  GetUserContract: function () {
    var returnStr = "0:6cabdc6646df86a378496e8ceb4f33b5a3f4808432bf02c816aff036dbe74110";


    var bufferSize = lengthBytesUTF8(returnStr) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(returnStr, buffer, bufferSize);
    return buffer;
  },

});