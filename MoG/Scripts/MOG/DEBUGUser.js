var MOG = 
    {
        DebugUser: function ()
        {
            var $btnChangeUser = $("#btnChangeUser");
            $btnChangeUser.on("click", function () {
                $.ajax({
                    url: '/DEBUG/SwitchUser',
                    dataType: 'json',
                    data : {"_nocache" :  new Date().getMilliseconds()},
                    contentType: 'application/json',
                    success: function (data) {
                        $btnChangeUser.html("User Switched");
                    }

                });
            });
        }
    }

$(document).ready(MOG.DebugUser());