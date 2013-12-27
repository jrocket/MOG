﻿function FileVM(dbFileId, FileStatus) {

    var self = this;

    this.comments = new ko.observableArray();
    this.newBody = new ko.observable();
    this.fileId = new ko.observable(dbFileId);
    this.DisplayMode = new ko.observable("DISPLAY");

    this.isDisplayMode = new ko.computed(function () {
        return self.DisplayMode() == "DISPLAY";
    });
    this.isEditMode = new ko.computed(function () {
        return self.DisplayMode() == "EDIT";
    });


    this.FileStatus = ko.observable(FileStatus);

    this.isBtnAcceptVisible = ko.computed(function () {
        return self.FileStatus() != 2;
    }, this);

    this.isBtnRejectVisible = ko.computed(function () {
        return self.FileStatus() != 3;
    }, this);

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
                alert(err.responseText);
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
                alert(err.responseText);
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
                    alert(err.responseText);
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

        var $taginputControl = $('[data-role=tagsinput]');
        var $elt = $taginputControl.tagsinput('input');
        if ($elt && $elt[0] && $elt[0].value != "") {
            //we have a pending tag, let's add it to the list
            if (/\S/.test($elt[0].value)) {
                // string is not empty and not just whitespace
                $taginputControl.tagsinput('add', $elt[0].value);
            }
        }

        var tags = $taginputControl.val();
        var description = $('#Description').val();
        var url = "/File/Edit/" + self.fileId();
        $.post(url,
            {
                "tags": tags,
                "description": description
            },
            function (data) {
                self.DisplayMode("DISPLAY");
                $("#mainContent").html(data);
            }
            )

    };

    this.btnCancelClicked = function () {
        self.DisplayMode("DISPLAY");
        var url = "/File/Detail/" + self.fileId();
        $.get(url, function (data) {
            $("#mainContent").html(data);
        })

    };

    $(document).ready(function () {
        console.dir(ko.toJSON({ "id": self.fileId(), "_nocache": new Date().getMilliseconds() }));
        $.ajax({
            url: '/File/GetComments',
            dataType: 'json',
            data: { "id": self.fileId(), "_nocache": new Date().getMilliseconds() },
            success: function (data) {
                self.comments(data);

            }
        });
    });


    this.btnCreateComment = function () {
        console.dir(self.fileId());
        $.ajax({
            url: '/Comment/Create',
            type: 'post',
            dataType: 'json',
            data: ko.toJSON({ "fileId": self.fileId(), "body": self.newBody() }),
            contentType: 'application/json',
            success: function (result) {
                self.comments.push(result);
                self.newBody("");

            },
            error: function (err) {
                if (err.responseText == "success")
                { window.location.href = urlPath + '/'; }
                else {
                    alert(err.responseText);
                }
            },
            complete: function () {
            }
        });
        return false;

    };



};






