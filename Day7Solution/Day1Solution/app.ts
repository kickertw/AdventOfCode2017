import fs = require('fs');

let buffer = fs.readFileSync('./input/input.txt', 'utf8').substr(1);
let skip = buffer.length / 2;
let answer = 0;

// console.log(buffer);
for (let ii = 0; ii < buffer.length; ii++) {
	var addAnswer = false;
	var skipIndex = ii + skip;
	if (skipIndex >= buffer.length) skipIndex -= buffer.length;

	// console.log('buffer[' + ii + '](' + buffer[ii] + ') <-> buffer[' + skipIndex + '](' + buffer[skipIndex] + ')');
	if (buffer[ii] === buffer[skipIndex]) {		
		addAnswer = true;
	}

	if (addAnswer) answer += parseInt(buffer[ii]);
}

console.log(answer);