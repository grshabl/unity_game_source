mergeInto(LibraryManager.library, {

  GetUserId: function () {
    return 64;
  },

  GetUserContract: function () {
    var returnStr = "0xaeae40b2Ea204e3Bc8e84aC2e8b26e2f75ab8391";


    var bufferSize = lengthBytesUTF8(returnStr) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(returnStr, buffer, bufferSize);
    return buffer;
  },

});