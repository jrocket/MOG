// ************** MESSAGE *********************//

function showMsgForm() {
    $("#pnlNewMessage").removeClass("hidden");
    $("#btnNewMessage").addClass("hidden");
    return false;
}
function hideMsgForm() {
    $("#pnlNewMessage").addClass("hidden");
    $("#btnNewMessage").removeClass("hidden");
    return false;
}



function MessageVM() {

    var self = this;


    self.folders = [i8n.MAIL_Inbox, i8n.MAIL_Sent, i8n.MAIL_Archive/*, 'Recherche','Live CHat'*/];

    self.controllerParam = new Object();
    self.controllerParam[self.folders[0]] = 'inbox';
    self.controllerParam[self.folders[1]] = 'outbox';
    self.controllerParam[self.folders[2]] = 'archive';
    //self.controllerParam[self.folders[3]] = 'Search';
    //self.controllerParam[self.folders[4]] = 'LiveChat';

    self.chosenFolderId = ko.observable();
    self.chosenFolderData = ko.observableArray();

    self.to = ko.observable();
    self.title = ko.observable();
    self.body = ko.observable();
    self.replyTo = ko.observable();
    self.displayReplyTo = ko.observable(true);
    self.Loading = ko.observable(false);

    // Behaviours
    self.goToFolder = function (folder) {
        self.chosenFolderId(folder);
        if (self.controllerParam[folder] != "inbox") {
            self.displayReplyTo(false);
        }
        else {
            self.displayReplyTo(true);
        }
        self.Loading(true);
        $.ajax({
            url: '/Message/GetFolder',
            dataType: 'json',
            contentType: 'application/json',
            data: { "folderName": self.controllerParam[folder], "_nocache": new Date().getMilliseconds() },
            success: function (data) {
                self.Loading(false);
                self.chosenFolderData(data);
            }
        });
    };

    self.btnArchiveClicked = function (data) {
        self.Loading(true);
        console.log(data.BoxId);
     
        var jsonPost = { "id": data.BoxId };
        $.ajax({
            type: "POST",
            url: '/Message/Archive',

            data: jsonPost,
            cache: false,
            success: function (data) {
                self.Loading(false);
                self.chosenFolderData.remove(function (item) { return item.Id == data.Id })
                
            }
        });
    };

    self.btnReplyClicked = function (data) {

        self.Loading(true);
        var jsonPost = { "id": data.Id, "_noCache": new Date().getMilliseconds() };
        $.ajax({
            type: "POST",
            url: '/Message/Detail',
            data: jsonPost,
            cache: false,
            success: function (result) {
                self.Loading(false);
                $('.TypeaheadInput').tagsinput('add', result.ReplyToLogin);
                self.title("RE:" + result.Title);
                var newBody = "\n------------\n";
                newBody = newBody + "From : " + result.Sender + "\n"
                newBody = newBody + "On  : " + result.SentOn + "\n"
                newBody = newBody + result.Body;
                self.body(newBody);
                self.replyTo(result.Id)
            }
        });
        showMsgForm();
    }

    self.btnDetailClicked = function (data) {
        self.Loading(true);

        var jsonPost = { "id": data.BoxId, "_noCache": new Date().getMilliseconds() };
        $.ajax({
            type: "POST",
            url: '/Message/Detail',

            data: jsonPost,
            cache: false,
            success: function (result) {
                self.Loading(false);
                var originaldata = self.chosenFolderData();
                for (var i = 0; i < originaldata.length; i++) {
                    if (originaldata[i].Id == result.Id) {

                        originaldata[i] = result;
                        break;
                    }
                }

                self.chosenFolderData(originaldata);
                //console.log(datas);
            }
        });
    }

    self.sendMail = function () {
        self.Loading(true);
        var jsonPost = { "to": self.to(), "title": self.title(), "body": self.body(), "replyTo": self.replyTo };

        $.ajax({
            type: "POST",
            url: '/Message/Send',

            data: jsonPost,
            cache: false,
            success: function (data) {
                self.Loading(false);
                self.to("");
                self.title("");
                self.body("");
                $('.TypeaheadInput').tagsinput('removeAll');
                var currentFolder = self.chosenFolderId();


                //self.chosenFolderData.unshift(data);
                $.ajax({
                    url: '/Message/GetFolder',
                    dataType: 'json',
                    contentType: 'application/json',
                    data: { "folderName": self.controllerParam[currentFolder], "_nocache": new Date().getMilliseconds() },
                    success: function (data) {
                        self.Loading(false);
                        self.chosenFolderData(data);

                    }
                });

            },
            error: function (data) {
                self.Loading(false);
                alert(i8n.MAIL_SendError);
            }
        });
    }
    //initialization
    self.goToFolder(self.folders[0]);
};

ko.applyBindings(new MessageVM());


//TYPEAHEAD
var elt = $('.TypeaheadInput');

elt.tagsinput();
elt.tagsinput('input').typeahead({
    prefetch: '/Message/GetFriends/1'
}).bind('typeahead:selected', $.proxy(function (obj, datum) {
    this.tagsinput('add', datum.value);
    this.tagsinput('input').typeahead('setQuery', '');
}, elt));
//ENDOF TYPEAHEAD

