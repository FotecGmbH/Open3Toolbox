window.SaveAs = function (filename, fileContent) {
    var link = document.createElement("a");
    link.download = filename;
    link.href = "data:text/plain;charset=utf-8," + encodeURIComponent(fileContent); //"data:application/octet-stream;base64,"
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}