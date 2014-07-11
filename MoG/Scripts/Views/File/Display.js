function Comments(model)
{
    var self = this;
    self.Body = ko.observable(model.Body);
    self.Creator = model.Creator;
    self.CreatorName = model.CreatorName;
    self.Id = model.Id;
    self.CreatedOnAsString = model.CreatedOnAsString;
    self.DeleteUrl = model.DeleteUrl;
    self.isEdit = ko.observable(false);
    self.isLoading = ko.observable(false);
    self.ModifiedOnAsString = model.ModifiedOnAsString;

    self.btnEditClicked = function ()
    {
        self.isEdit(true);
    }

    self.btnSaveClicked = function (data)
    {
        self.isEdit(false);
        self.isLoading(true);

        $.ajax({
            url: '/Comment/Edit',
            type: 'post',
            dataType: 'json',
            data: ko.toJSON({ "id": self.Id, "body" : self.Body }),
            contentType: 'application/json',
            success: function (result) {
                self.isLoading(false);
                if (result.data == true) {//Set promoted 
                    
                }
                else
                {
                    ns_MOG.displayModal("Error", result.message);
                   
                }
            },
            error: function (err) {
                self.isLoading(false);
                ns_MOG.displayModal("Error", err.responseText);
               
            }
        });
        //console.dir(data);
       
    }
}



function FileVM(dbFileId, FileStatus, Promoted) {

    var self = this;

    this.comments =  ko.observableArray();
    this.newBody =  ko.observable();
    this.fileId =  ko.observable(dbFileId);
    this.DisplayMode =  ko.observable("DISPLAY");
    this.isAddToCartVisible =  ko.observable(true);
    this.isPromoteVisible = ko.observable(Promoted === "False");

    this.isDisplayMode =  ko.computed(function () {
        return self.DisplayMode() == "DISPLAY";
    });
    this.isEditMode =  ko.computed(function () {
        return self.DisplayMode() == "EDIT";
    });


    this.FileStatus = ko.observable(FileStatus);

    this.isBtnAcceptVisible = ko.computed(function () {
        return self.FileStatus() != 2;
    }, this);

    this.isBtnRejectVisible = ko.computed(function () {
        return self.FileStatus() != 3;
    }, this);

    this.btnPromoteClicked = function () {
        $.ajax({
            url: '/Project/Promote',
            type: 'post',
            dataType: 'json',
            data: ko.toJSON({ "fileId": self.fileId }),
            contentType: 'application/json',
            success: function (result) {

                if (result == true) {//Set promoted 
                    self.isPromoteVisible(false);
                }
            },
            error: function (err) {
                ns_MOG.displayModal("Error", err.responseText);
               
            }
        });
    }

    this.btnAcceptClicked = function () {
        $.ajax({
            url: '/File/Accept',
            type: 'post',
            dataType: 'json',
            data: ko.toJSON({ "fileId": self.fileId }),
            contentType: 'application/json',
            success: function (result) {

                if (result == true) {//Set status to accepted
                    self.FileStatus(2);
                }
            },
            error: function (err) {
                ns_MOG.displayModal("Error", err.responseText);
               
            }
        });
    };

    this.btnRejectClicked = function () {
        $.ajax({
            url: '/File/Reject',
            type: 'post',
            dataType: 'json',
            data: ko.toJSON({ "fileId": self.fileId }),
            contentType: 'application/json',
            success: function (result) {

                if (result == true) {//Set status to rejected
                    self.FileStatus(3);
                }
            },
            error: function (err) {
                ns_MOG.displayModal("Error", err.responseText);
               
            }
        });
    };


    this.btnDeleteClicked = function () {
        var confirmed = confirm("Are you sure?");
        if (confirmed) {
            $.ajax({
                url: '/File/Delete',
                type: 'post',
                dataType: 'json',
                data: ko.toJSON({ "fileId": self.fileId }),
                contentType: 'application/json',
                success: function (result) {

                    if (result.Flag == true) {//Set status to accepted
                        window.location.href = result.Url;
                    }
                },
                error: function (err) {
                    ns_MOG.displayModal("Error", err.responseText);
                  
                }
            });
        }

    };

    this.btnEditClicked = function () {
        self.DisplayMode("EDIT");
        var url = "/File/Edit/" + self.fileId();
        $.get(url, function (data) {
            $("#mainContent").html(data);
        })

    };

    this.btnSaveClicked = function () {

        
        //if ($elt && $elt[0] && $elt[0].value != "") {
        //    //we have a pending tag, let's add it to the list
        //    if (/\S/.test($elt[0].value)) {
        //        // string is not empty and not just whitespace
        //        $taginputControl.tagsinput('add', $elt[0].value);
        //    }
        //}

        var tags = $('#Tags').val();
        var description = $('#Description').val();
        var name = $('#DisplayName').val();
        var url = "/File/Edit/" + self.fileId();
        $.post(url,
            {
                "tags": tags,
                "description": description,
                "name": name
            },
            function (data) {
                self.DisplayMode("DISPLAY");
                $("#mainContent").html(data);
            }
            );

    };

    this.btnCancelClicked = function () {
        self.DisplayMode("DISPLAY");
        var url = "/File/Detail/" + self.fileId();
        $.get(url, function (data) {
            $("#mainContent").html(data);
        })

    };

    this.btnAddToCart = function () {
        var url = "/DownloadCart/Create"
        $.post(url,
          {
              "fileId": self.fileId()

          },
          function (data) {
              self.isAddToCartVisible(false);
              ns_MOG.displayModal("Success", "File has been added to your cart");
             
          }
          );

    };

    $(document).ready(function () {
        //console.dir(ko.toJSON({ "id": self.fileId(), "_nocache": new Date().getMilliseconds() }));
        $.ajax({
            url: '/File/GetComments',
            dataType: 'json',
            data: { "id": self.fileId(), "_nocache": new Date().getMilliseconds() },
            success: function (data) {
                var viewmodel = [];
                for (var i = 0; i < data.length; i++)
                {
                    viewmodel[i] = new Comments(data[i]);
                }
                self.comments(viewmodel);

            }
        });
    });


    this.btnDeleteComment = function (data) {
        if (!confirm('Sure, you\'re sure?'))
        {
            return;
        }
        $.ajax({
            url: '/Comment/Delete',
            type: 'post',
            dataType: 'json',
            data: ko.toJSON({ "id": data.Id }),
            contentType: 'application/json',
            success: function (result) {
                self.comments.remove(data);
              
            },
            error: function (err) {
                if (err.responseText == "success")
                { window.location.href = urlPath + '/'; }
                else {
                    ns_MOG.displayModal("Error", err.responseText);
                  
                }
            },
            complete: function () {
            }
        })
    }

    this.btnCreateComment = function () {
        //console.dir(self.fileId());
        $.ajax({
            url: '/Comment/Create',
            type: 'post',
            dataType: 'json',
            data: ko.toJSON({ "fileId": self.fileId(), "body": self.newBody() }),
            contentType: 'application/json',
            success: function (result) {
                var newComment = new Comments(result);
                self.comments.push(newComment);
                self.newBody("");

            },
            error: function (err) {
                if (err.responseText == "success")
                { window.location.href = urlPath + '/'; }
                else {
                    ns_MOG.displayModal("Error", err.responseText);
                   
                }
            },
            complete: function () {
            }
        });
        return false;

    };

    $('#buttonRow').show();

};






