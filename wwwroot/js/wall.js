// js here


function startConverting () {
    if('webkitSpeechRecognition' in window){
        var speechRecognizer = new webkitSpeechRecognition();
        speechRecognizer.continuous = false;
        speechRecognizer.interimResults = false;
        speechRecognizer.lang = 'en-IN';
        speechRecognizer.start();

        var finalTranscripts = '';

        speechRecognizer.onresult = function(event){
            var interimTranscripts = '';
            for(var i = event.resultIndex; i < event.results.length; i++){
                var transcript = event.results[i][0].transcript;
                transcript.replace("\n", "<br>");
                if(event.results[i].isFinal){
                    finalTranscripts += transcript;
                }else{
                    interimTranscripts += transcript;
                }
            }

            //make api call with start
            // key1: 8f93afbf3c0940bc9e7542d33a971c99
            var xhr = new XMLHttpRequest();
            xhr.open("GET", "https://api.projectoxford.ai/luis/v1/application?id=fae908e1-61ef-49c0-98cc-4c832ce7fc29&subscription-key=29768a93470c4ccda7d46c7023d6adc7&q=" + finalTranscripts, false);
            xhr.onload = function (e) {
                if (xhr.readyState === 4) {
                    if (xhr.status === 200) {

                        var data = JSON.parse(xhr.responseText);
                        console.log(data);

                        if(data['intents'][0]['intent'] == "AddMessage"){

                            if(data['intents'][0]['actions'][0]['parameters'][0]['value']!= null){
                                var message = data['intents'][0]['actions'][0]['parameters'][0]['value'][0]['entity'];
                                var textarea = $('#new_message textarea');
                                textarea.val(message);
                                $('#new_message form').submit();
                            }else{
                                alert('Sorry, message was not recognized. Please try again.')
                            }


                        }else if(data['intents'][0]['intent'] == "PostComment"){

                            if(data['intents'][0]['actions'][0]['parameters'][0]['value'] != null){
                                var comment = data['intents'][0]['actions'][0]['parameters'][0]['value'][0]['entity'];
                                if(data['entities'][0]['entity'] != null){
                                    var id = data['entities'][0]['entity']
                                    var textarea = $('.new_comment textarea[id="'+ id + '"]');
                                    textarea.val(comment);
                                    $('.new_comment textarea[id="'+ id + '"]').parent().submit();
                                }else{
                                    alert('Sorry, id was not recognized. Please try again.');
                                }
                            }else{
                                alert('Sorry, comment was not recognized. Please try again.');
                            }

                        }else if(data['intents'][0]['intent'] == "SignOut"){

                            window.location.href = "/logoff";

                        }else{
                            alert("Command not recognized. Sorry, please try again.");
                        }
                    } else {
                        console.error(xhr.statusText);
                        alert("Bad connection. Please try again.")
                    }
                }
            };
            xhr.onerror = function (e) {
                console.error(xhr.statusText);
            };
            xhr.send(null);
        };
        speechRecognizer.onerror = function (event) {
        };
    }else{
        document.alert("Your browser does not support voice recognition");
    }
}