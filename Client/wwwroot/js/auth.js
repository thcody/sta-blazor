window.getJwtToken = function() {
    const name = "StaticWebAppsAuthCookie=";
    const decodedCookie = decodeURIComponent(document.cookie);
    
    return decodedCookie;
}