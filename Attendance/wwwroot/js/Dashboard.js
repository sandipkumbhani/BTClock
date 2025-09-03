
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
		fetch(`/Attendance/GetLastFiveRecords?employeeId=${employeeId}`)
			.then(res => res.json())
			.then(data => {
				attendanceRecords = data;
				renderAttendanceTable();
			});
		}

	function renderAttendanceTable() {
			const tbody = document.getElementById('attendanceTableBody');
	tbody.innerHTML = '';
			attendanceRecords.forEach(r => {
				const tr = document.createElement('tr');
	tr.innerHTML = `<td>${new Date(r.clockIn).toLocaleDateString()}</td>
	<td>${new Date(r.clockIn).toLocaleTimeString()}</td>
	<td>${r.clockOut ? new Date(r.clockOut).toLocaleTimeString() : ''}</td>
	<td>${r.clockOut ? getDuration(r.clockIn, r.clockOut) : ''}</td>`;
	tbody.appendChild(tr);
			});
		}

		// function renderAttendanceTable() {
		// 	const tbody = document.getElementById('attendanceTableBody');
		// 	tbody.innerHTML = '';
		// 	Show only today's records, up to 4 sessions
		// 	let todaysRecords = attendanceRecords.filter(r => new Date(r.clockIn).toLocaleDateString() === todayStr);
		// 	todaysRecords.slice(-5).forEach(r => {
		// 		const tr = document.createElement('tr');
		// 		tr.innerHTML = `<td>${new Date(r.clockIn).toLocaleDateString()}</td><td>${new Date(r.clockIn).toLocaleTimeString()}</td><td>${r.clockOut ? new Date(r.clockOut).toLocaleTimeString() : ''}</td><td>${r.clockOut ? getDuration(r.clockIn, r.clockOut) : ''}</td>`;
		// 		tbody.appendChild(tr);
		// 	});
		// }

		function getDuration(clockIn, clockOut) {
			let diffMs = new Date(clockOut) - new Date(clockIn);
			let diffHrs = Math.floor(diffMs / 3600000);
			let diffMins = Math.floor((diffMs % 3600000) / 60000);
			let diffSecs = Math.floor((diffMs % 60000) / 1000);
			return `${diffHrs}h ${diffMins}m ${diffSecs}s`;
		}

		function fetchAttendance(IsLiveInterval) {
		fetch('/Attendance/Report?employeeId=${employeeId}')
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

	document.getElementById('clockInBtn').addEventListener('click', function() {
		fetch('/Attendance/ClockIn', {
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
				document.getElementById('error-message').style.display = 'none';
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

	document.getElementById('clockOutBtn').addEventListener('click', function() {
		fetch('/Attendance/ClockOut', {
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
				document.getElementById('endTime').textContent = " | Clock Out: " + sessionEnd.toLocaleTimeString();
				document.getElementById('workDuration').textContent = getDuration(record.clockIn, record.clockOut);
				sessionStart = null;
				updateButtonStates();
				clearInterval(liveInterval);
				document.getElementById('liveCounter').textContent = "";
				fetchAttendance(1);
			});
		});


	// After DOM is ready, check backend status and fetch attendance
	window.addEventListener('DOMContentLoaded', function() {
		fetchAttendance(1);
	updateButtonStates();
	});


