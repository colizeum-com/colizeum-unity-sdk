mergeInto(LibraryManager.library, {
    OpenWindow: function (url, target, features) => {
        window.open(url, target, features);
    }
});