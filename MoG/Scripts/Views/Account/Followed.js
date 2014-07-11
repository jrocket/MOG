function FollowedVM(options) {
    var self = this;
    self.followed = ko.observableArray();

    self.btnUnfollowClicked = function (data) {
        
        var url = options.urlUnfollow;
        var data = { id: data.ProjectId };
        $.post(url, data, function (result) {
            if (result) {
                ns_MOG.displayModal("Success", "successfully unfollowed");
                
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
