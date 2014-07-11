function ManageVM(options) {
    var self = this;
    self.isEditMode = ko.observable(false);
    self.displayName = ko.observable(options.displayName)
    self.email = ko.observable(options.email);
    self.id = ko.observable(options.id);
   

    self.btnEditClicked = function () {
        self.isEditMode(true);
    };

    self.btnSaveClicked = function () {

       
        var url = options.urlSaveProfile;
            var data = { id: self.id(), displayName :self.displayName(), email : self.email() };
            $.post(url, data, function (result) {
                if (result.Result) {
                    ns_MOG.displayModal("whoot ! ", " :)");
                   
                    self.isEditMode(false);
                }
                else
                {
                    ns_MOG.displayModal("erk ! ", " :(");
                }

            });
       
    }

    // 
    //}

    //self.getData = function () {
    //    $.post("/follow/GetFollowed", null, function (data) {
    //        self.followed(data);
    //    });
    //};
    //self.getData();

}
