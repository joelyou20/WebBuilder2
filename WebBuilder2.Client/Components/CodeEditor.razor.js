﻿function ace() {
    ace.edit("example", {
        theme: "ace/theme/textmate",
        mode: "ace/mode/javascript",
        value: "console.log('Hello world');"
    });
}

ace()