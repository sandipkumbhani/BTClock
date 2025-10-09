let liveInterval;
let sessionStart = null;
let attendanceRecords = [];
let todayStr = new Date().toLocaleDateString();
function updateButtonStates() {
	if (sessionStart) {
		document.getElementById('clockInBtn').hidden = true;
		document.getElementById('clockOutBtn').hidden = false;
	} else {
		document.getElementById('clockInBtn').hidden = false;
		document.getElementById('clockOutBtn').hidden = true;
	}
}
function fetchAttendance() {
	fetch(`/Attendance/GetLastFiveRecords?userId=${userId}`)
		.then(res => res.json())
		.then(data => {
			attendanceRecords = data;
			renderAttendanceTable();
		});
}
function getDuration(clockIn, clockOut) {
    if (!clockIn || !clockOut) return '0h 0m 0s';
    let diffMs = new Date(clockOut) - new Date(clockIn);
    let diffHrs = Math.floor(diffMs / 3600000);
    let diffMins = Math.floor((diffMs % 3600000) / 60000);
    let diffSecs = Math.floor((diffMs % 60000) / 1000);
    return `${diffHrs}h ${diffMins}m ${diffSecs}s`;
}


function renderAttendanceTable() {
	const tbody = document.getElementById('attendanceTableBody');
	tbody.innerHTML = '';

	const sortedRecords = attendanceRecords
		.sort((a, b) => new Date(b.clockIn) - new Date(a.clockIn))
		.slice(0, 5);

	sortedRecords.forEach(r => {
		const hasClockOut = r.clockOut !== null;

		const tr = document.createElement('tr');
		tr.innerHTML = `
			<td>${new Date(r.clockIn).toLocaleDateString()}</td>
			<td>${new Date(r.clockIn).toLocaleTimeString()}</td>
			<td>${hasClockOut ? new Date(r.clockOut).toLocaleTimeString() : ''}</td>
			<td>${hasClockOut && r.overtimeHours ? formatTimeSpan(r.overtimeHours) : ''}</td>
			<td>${hasClockOut && r.workingHour ? formatTimeSpan(r.workingHour) : ''}</td>
		`;
		tbody.appendChild(tr);
	});
}

function formatTimeSpan(timeSpan) {
	if (!timeSpan || timeSpan === "00:00:00") {
		return "0h 0m 0s";
	}
	const [hours, minutes, seconds] = timeSpan.split(':');
	return `${parseInt(hours)}h ${parseInt(minutes)}m ${parseInt(seconds)}s`;
}


function fetchAttendance(IsLiveInterval = 1) {
	fetch(`/Attendance/Report?userId=${userId}`)
		.then(res => res.json())
		.then(data => {
			attendanceRecords = data;
			renderAttendanceTable();
			clearInterval(liveInterval);
			document.getElementById('liveCounter').textContent = "";

			data.forEach(x => {
				if (x.clockOut == null) {
					sessionStart = new Date(x.clockIn);
					let SessionStartTime = sessionStart;
					if (IsLiveInterval == 0) {
						SessionStartTime = new Date();
					}
					liveInterval = setInterval(function () {
						let now = new Date();
						let diffMs = now - SessionStartTime;
						let diffHrs = Math.floor(diffMs / 3600000);
						let diffMins = Math.floor((diffMs % 3600000) / 60000);
						let diffSecs = Math.floor((diffMs % 60000) / 1000);
						document.getElementById('liveCounter').textContent = `Working: ${diffHrs}h ${diffMins}m ${diffSecs}s`;
					}, 1000);
				}

			});

			updateButtonStates();
		});
}

function IsUserClockedIn() {
	fetch('/Attendance/IsUserClockedIn')
		.then(res => res.json())
		.then(data => {
			debugger;
			if (data) {

				document.getElementById('clockInBtn').hidden = true;
				document.getElementById('clockOutBtn').hidden = false;
			} else {
				document.getElementById('clockInBtn').hidden = false;
				document.getElementById('clockOutBtn').hidden = true;
			}
		});
}

document.getElementById('clockInBtn').addEventListener('click', function () {
	fetch('/Attendance/ClockInAttendance', {
		method: 'GET',
		headers: { 'Content-Type': 'application/json' }
	})
		.then(async res => {
			if (!res.ok) {
				const error = await res.json().catch(() => ({ error: 'Unknown error' }));
				document.getElementById('error-message').innerText = error.error || '';
				document.getElementById('error-message').style.display = 'inline';
				return; // Do not throw, just stop here
			}
			document.getElementById('error-message').style.display = 'inline';
			return res.json();
		})
		.then(record => {
			sessionStart = new Date(record.clockIn);
			document.getElementById('startTime').textContent = "Clock In: " + sessionStart.toLocaleTimeString();
			document.getElementById('endTime').textContent = "";
			document.getElementById('workDuration').textContent = "";
			fetchAttendance(0);
			updateButtonStates();
		});
});

document.getElementById('clockOutBtn').addEventListener('click', function () {
	fetch('/Attendance/ClockOutAttendance', {
		method: 'GET',
		headers: { 'Content-Type': 'application/json' }
	})
		.then(async res => {
			if (!res.ok) {
				const error = await res.json().catch(() => ({ error: 'Unknown error' }));
				document.getElementById('error-message').innerText = error.error || '';
				document.getElementById('error-message').style.display = 'inline';
				return;
			}
			document.getElementById('error-message').style.display = 'none';
			return res.json();
		})
		.then(record => {
			if (!record || !record.clockOut) return;
			let sessionEnd = new Date(record.clockOut);
			document.getElementById('endTime').textContent = "  Clock Out: " + sessionEnd.toLocaleTimeString();
			document.getElementById('workDuration').textContent = getDuration(record.clockIn, record.clockOut);
			sessionStart = null;
			updateButtonStates();
			clearInterval(liveInterval);
			document.getElementById('liveCounter').textContent = "";
			fetchAttendance(1);
		});
});


// After DOM is ready, check backend status and fetch attendance
window.addEventListener('DOMContentLoaded', function () {
	fetchAttendance(1);
	updateButtonStates();
});



