// call get response from Flickr API
function getResponse(url) {
    var rt = $.ajax({
        type: "POST",
        url: url,
        async: false,
        dataType: "json",
        success: function (response) {
            return response;
        }
    }).responseText;
    var obj = jQuery.parseJSON(rt);
    return obj;
}

// uses Flickr API to get user's photos
function getImages(sectNum, apiKey, url, photosetId, perPage) {

    var getUserApi = "https://api.flickr.com/services/rest/?method=flickr.urls.lookupUser&api_key=" + apiKey + "&url=" + url + "&format=json&nojsoncallback=1";
    var userObj = getResponse(getUserApi);

    var getPhotosApi = "https://api.flickr.com/services/rest/?method=flickr.photosets.getPhotos&api_key=" + apiKey + "&user_id=" + userObj.user.id + "&photoset_id=" + photosetId + "&privacy_filter=1&per_page=" + perPage + "&format=json&nojsoncallback=1";
    var photosetObj = getResponse(getPhotosApi);

    $.each(photosetObj.photoset.photo, function (i, item) {
        var imgSrc = "http://c1.staticflickr.com/" + item.farm + "/" + item.server + "/" + item.id + "_" + item.secret + "_c.jpg";
        var image = $("<img/>").attr("src", imgSrc).attr("alt", item.title);        
        
        $(image).appendTo("#sect" + sectNum);

    });

    var text = $("<p>").html("View more photos from " + userObj.user.username._content + "'s Flickr album: ");
    var link = $("<a>").attr("href", url).attr("target", "_blank").html(photosetObj.photoset.title);
    $(link).appendTo($(text).insertAfter($("<br>").insertAfter("#sect" + sectNum)));
}