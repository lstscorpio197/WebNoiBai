function HttpMessageClientRequestDto(data, description) {
    this.Header = new ClientRequestHeader();
    this.Body = new ClientRequestBody(data, description);
    return this;
}
function ClientRequestHeader() {
    this.SenderKey = null;
    this.SenderCode = null;
    this.SenderDate = null;
    this.TransationID = null;
    this.Token = sessionStorage.getItem(TOKEN_STORAGE_NAME);
    this.RefreshToken = sessionStorage.getItem(REFRESHTOKEN_STORAGE_NAME);
    return this;
}
function ClientRequestBody(data, description) {
    this.Data = data;
    this.Description = description;
    return this;
}