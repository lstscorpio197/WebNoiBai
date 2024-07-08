function MessageDto(_isOk) {
    this.IsOk = (_isOk || false);
    this.Data = null;
    this.SubData = null;
    this.Description = null;
    this.Code = null;
    this.Name = null;
    return this;
}