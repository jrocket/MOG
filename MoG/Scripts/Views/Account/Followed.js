function FollowedVM(options) {
    var self = this;
    self.followed = ko.observableArray();

    self.btnUnfollowClicked = function (data) {
        console.dir(data);
        var url = urlUnfollow.urlUnfollow;
        var data = { id: data.ProjectId };
        $.post(url, data, function (result) {
            if (result) {
                alert("successfully unfollowed");
                self.getData();
            }
        });
    }

    self.getData = function () {
        $.post("/follow/GetFollowed", null, function (data) {
            self.followed(data);
        });
    };
    self.getData();

}
