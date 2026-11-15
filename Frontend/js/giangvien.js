// ===== GIẢNG VIÊN PAGE JS =====
if (!Auth.isLoggedIn()) window.location.href = 'index.html';
renderUserInfo();

// Lấy mã giảng viên từ user session
const user = Auth.getUser();
const maGV = user?.maGiangVien || user?.userName || user?.tenDangNhap || '';

let lichDayData = [];
let diemDanhSVList = [];
let currentLichHocId = null;
let profileData = null;

// ===== SIDEBAR NAV =====
document.querySelectorAll('.nav-item').forEach(item => {
  item.addEventListener('click', () => {
    const page = item.dataset.page;
    document.querySelectorAll('.nav-item').forEach(i => i.classList.remove('active'));
    document.querySelectorAll('.page').forEach(p => p.classList.remove('active'));
    item.classList.add('active');
    const pageEl = document.getElementById(page);
    if (pageEl) pageEl.classList.add('active');
    
    const titles = {
      'page-dashboard': 'Dashboard',
      'page-profile': 'Thông tin cá nhân',
      'page-lichday': 'Lịch giảng dạy',
      'page-diemdanh': 'Quản lý điểm danh',
    };
    document.getElementById('page-title').textContent = titles[page] || '';
    
    if (page === 'page-dashboard') loadDashboard();
    else if (page === 'page-profile') loadProfile();
    else if (page === 'page-lichday') loadLichDay();
    else if (page === 'page-diemdanh') {
      initTabs('#page-diemdanh');
      loadDiemDanhLich();
      loadAllDiemDanh();
    }
  });
});

// ===== DASHBOARD =====
async function loadDashboard() {
  try {
    if (!maGV) {
      document.getElementById('dash-upcoming').innerHTML = '<div class="info-box warning">⚠️ Không tìm được mã giảng viên. Hãy kiểm tra thông tin đăng nhập.</div>';
      return;
    }
    
    const data = await apiND(`/api/nguoidung/lichgiangday/${maGV}`);
    lichDayData = data?.data || data || [];
    
    const today = new Date(); today.setHours(0,0,0,0);
    const todayStr = today.toISOString().split('T')[0];
    const buoiHomNay = lichDayData.filter(l => l.ngayHoc && l.ngayHoc.startsWith(todayStr));
    
    document.getElementById('dash-so-buoi').textContent = buoiHomNay.length;
    document.getElementById('dash-tong-lich').textContent = lichDayData.length;
    
    // Sắp tới = từ hôm nay trở đi
    const upcoming = lichDayData
      .filter(l => l.ngayHoc && l.ngayHoc >= todayStr)
      .sort((a, b) => a.ngayHoc.localeCompare(b.ngayHoc))
      .slice(0, 8);
    
    if (!upcoming.length) {
      document.getElementById('dash-upcoming').innerHTML = '<div class="empty-state"><div class="empty-icon">📅</div><h3>Không có lịch dạy sắp tới</h3></div>';
      return;
    }
    
    document.getElementById('dash-upcoming').innerHTML = upcoming.map(l => {
      const dt = new Date(l.ngayHoc);
      const days = ['CN','T2','T3','T4','T5','T6','T7'];
      return `<div class="schedule-item">
        <div class="schedule-date">
          <div class="day-num">${dt.getDate()}</div>
          <div class="day-name">${days[dt.getDay()]}</div>
          <div class="month">Th${dt.getMonth()+1}</div>
        </div>
        <div class="schedule-info">
          <div class="schedule-subject">${l.maLopHocPhan || l.tenMonHoc || 'Lớp HP: ' + l.maLopHocPhan}</div>
          <div class="schedule-meta">
            <span>⏰ ${fmtTime(l.gioBatDau)} - ${fmtTime(l.gioKetThuc)}</span>
            ${l.maPhong ? `<span>🏫 Phòng ${l.maPhong}</span>` : ''}
            <span class="badge badge-active">${l.trangThai || 'ChuaBatDau'}</span>
          </div>
        </div>
        <div>
          <button class="btn btn-primary btn-sm" onclick="goToDiemDanh(${l.maLichHoc})">✅ Điểm danh</button>
        </div>
      </div>`;
    }).join('');
    
    document.getElementById('dash-da-diemdanh').textContent = '—';
  } catch (e) {
    document.getElementById('dash-upcoming').innerHTML = `<div class="info-box danger">❌ ${e.message}</div>`;
  }
}

function goToDiemDanh(maLichHoc) {
  // Chuyển sang tab điểm danh
  document.querySelectorAll('.nav-item').forEach(i => i.classList.remove('active'));
  document.querySelectorAll('.page').forEach(p => p.classList.remove('active'));
  document.querySelector('[data-page="page-diemdanh"]').classList.add('active');
  document.getElementById('page-diemdanh').classList.add('active');
  document.getElementById('page-title').textContent = 'Quản lý điểm danh';
  
  initTabs('#page-diemdanh');
  
  // Chuyển sang tab chi tiết
  document.querySelectorAll('#page-diemdanh .tab-btn').forEach(b => b.classList.remove('active'));
  document.querySelectorAll('#page-diemdanh .tab-content').forEach(c => c.classList.remove('active'));
  document.querySelector('[data-tab="tab-diemdanh-chitiet"]').classList.add('active');
  document.getElementById('tab-diemdanh-chitiet').classList.add('active');
  
  document.getElementById('dd-manual-lichid').value = maLichHoc;
  loadDiemDanhChiTiet(maLichHoc);
}

// ===== THÔNG TIN CÁ NHÂN =====
async function loadProfile() {
  document.getElementById('profile-view').innerHTML = '<div class="loading"><div class="spinner"></div></div>';
  try {
    const data = await apiAdmin(`/api/admin/giangvien/${maGV}`);
    profileData = data;
    renderProfileView(data);
    // Set edit fields
    document.getElementById('prof-ma').value = data.maGiangVien;
    document.getElementById('prof-hoten').value = data.hoTen;
    document.getElementById('prof-email').value = data.email || '';
    document.getElementById('prof-sdt').value = data.sDT || data.sdt || '';
    document.getElementById('prof-khoa').value = data.khoa || '';
  } catch (e) {
    document.getElementById('profile-view').innerHTML = `<div class="info-box danger">❌ Không thể tải thông tin: ${e.message}</div>`;
  }
}

function renderProfileView(d) {
  document.getElementById('profile-view').innerHTML = `
    <div style="display:flex;align-items:center;gap:20px;margin-bottom:24px;">
      <div style="width:72px;height:72px;border-radius:50%;background:linear-gradient(135deg,#0891b2,#2563eb);display:flex;align-items:center;justify-content:center;font-size:28px;font-weight:800;color:white;">${(d.hoTen||'G').charAt(0)}</div>
      <div>
        <div style="font-size:20px;font-weight:800;">${d.hoTen}</div>
        <div style="font-size:13px;color:#64748b;">Giảng viên - ${d.khoa || 'Chưa cập nhật khoa'}</div>
        <div style="margin-top:4px;">${fmtTrangThai(d.trangThai)}</div>
      </div>
    </div>
    <div style="display:grid;grid-template-columns:1fr 1fr;gap:16px;">
      <div><div style="font-size:11px;font-weight:700;color:#64748b;text-transform:uppercase;margin-bottom:4px;">Mã giảng viên</div><div style="font-size:14px;font-weight:600;">${d.maGiangVien}</div></div>
      <div><div style="font-size:11px;font-weight:700;color:#64748b;text-transform:uppercase;margin-bottom:4px;">Email</div><div style="font-size:14px;">${d.email || '—'}</div></div>
      <div><div style="font-size:11px;font-weight:700;color:#64748b;text-transform:uppercase;margin-bottom:4px;">Số điện thoại</div><div style="font-size:14px;">${d.sDT || d.sdt || '—'}</div></div>
      <div><div style="font-size:11px;font-weight:700;color:#64748b;text-transform:uppercase;margin-bottom:4px;">Khoa</div><div style="font-size:14px;">${d.khoa || '—'}</div></div>
    </div>`;
}

function toggleEditProfile() {
  const view = document.getElementById('profile-view');
  const edit = document.getElementById('profile-edit');
  const isEditing = edit.style.display !== 'none';
  if (isEditing) {
    edit.style.display = 'none';
    view.style.display = '';
    if (profileData) renderProfileView(profileData);
  } else {
    view.style.display = 'none';
    edit.style.display = '';
  }
}

async function saveProfile() {
  if (!profileData) return;
  const body = {
    ...profileData,
    hoTen: document.getElementById('prof-hoten').value.trim(),
    email: document.getElementById('prof-email').value.trim() || null,
    sDT: document.getElementById('prof-sdt').value.trim() || null,
    khoa: document.getElementById('prof-khoa').value.trim() || null,
  };
  if (!body.hoTen) { showToast('Vui lòng nhập họ tên', 'warning'); return; }
  
  try {
    await apiAdmin(`/api/admin/giangvien/${maGV}`, 'PUT', body);
    profileData = body;
    showToast('Cập nhật thông tin thành công!', 'success');
    toggleEditProfile();
  } catch (e) {
    showToast('Lỗi: ' + e.message, 'error');
  }
}

// ===== LỊCH GIẢNG DẠY =====
async function loadLichDay() {
  document.getElementById('lichday-list').innerHTML = '<div class="loading"><div class="spinner"></div></div>';
  try {
    if (!maGV) throw new Error('Không tìm được mã giảng viên');
    const data = await apiND(`/api/nguoidung/lichgiangday/${maGV}`);
    lichDayData = data?.data || data || [];
    renderLichDay(lichDayData);
  } catch (e) {
    document.getElementById('lichday-list').innerHTML = `<div class="info-box danger">❌ ${e.message}</div>`;
  }
}

function renderLichDay(data) {
  if (!data.length) {
    document.getElementById('lichday-list').innerHTML = '<div class="empty-state"><div class="empty-icon">📅</div><h3>Chưa có lịch giảng dạy</h3><p>Liên hệ admin để được phân công</p></div>';
    return;
  }
  
  const days = ['Chủ nhật','Thứ 2','Thứ 3','Thứ 4','Thứ 5','Thứ 6','Thứ 7'];
  document.getElementById('lichday-list').innerHTML = data.map(l => {
    const dt = new Date(l.ngayHoc);
    return `<div class="schedule-item">
      <div class="schedule-date">
        <div class="day-num" style="color:var(--gv-color)">${dt.getDate()}</div>
        <div class="day-name">${days[dt.getDay()]}</div>
        <div class="month">Tháng ${dt.getMonth()+1}/${dt.getFullYear()}</div>
      </div>
      <div class="schedule-info">
        <div class="schedule-subject">📚 ${l.maLopHocPhan || '—'}</div>
        <div class="schedule-meta">
          <span>⏰ ${fmtTime(l.gioBatDau)} – ${fmtTime(l.gioKetThuc)}</span>
          ${l.maPhong ? `<span>🏫 Phòng ${l.maPhong}</span>` : '<span>🏫 Chưa có phòng</span>'}
          <span>${fmtTrangThaiLich(l.trangThai)}</span>
        </div>
      </div>
      <div>
        <button class="btn btn-primary btn-sm" onclick="goToDiemDanh(${l.maLichHoc})">✅ Điểm danh</button>
      </div>
    </div>`;
  }).join('');
}

function fmtTrangThaiLich(t) {
  const map = {
    'ChuaBatDau': '<span class="badge badge-pending">Chưa bắt đầu</span>',
    'DangDienRa': '<span class="badge badge-approved">Đang diễn ra</span>',
    'DaKetThuc': '<span class="badge badge-inactive">Đã kết thúc</span>',
    'HuyBo': '<span class="badge badge-rejected">Hủy bỏ</span>',
  };
  return map[t] || `<span class="badge">${t || '—'}</span>`;
}

function filterLichDay() {
  const filter = document.getElementById('ld-filter-tuan').value;
  if (!filter) { renderLichDay(lichDayData); return; }
  
  const now = new Date(); now.setHours(0,0,0,0);
  const thisMonday = new Date(now); thisMonday.setDate(now.getDate() - now.getDay() + 1);
  const thisSunday = new Date(thisMonday); thisSunday.setDate(thisMonday.getDate() + 6);
  const nextMonday = new Date(thisMonday); nextMonday.setDate(thisMonday.getDate() + 7);
  const nextSunday = new Date(nextMonday); nextSunday.setDate(nextMonday.getDate() + 6);
  
  const filtered = lichDayData.filter(l => {
    if (!l.ngayHoc) return false;
    const d = new Date(l.ngayHoc);
    if (filter === 'this') return d >= thisMonday && d <= thisSunday;
    if (filter === 'next') return d >= nextMonday && d <= nextSunday;
    return true;
  });
  renderLichDay(filtered);
}

// ===== ĐIỂM DANH =====
async function loadDiemDanhLich() {
  document.getElementById('dd-lich-table').innerHTML = '<tr><td colspan="7" style="text-align:center;padding:20px"><div class="spinner" style="margin:auto"></div></td></tr>';
  try {
    // Dùng lịch giảng dạy của giảng viên
    if (!maGV) throw new Error('Không tìm được mã GV');
    const data = await apiND(`/api/nguoidung/lichgiangday/${maGV}`);
    const lich = data?.data || data || [];
    
    if (!lich.length) {
      document.getElementById('dd-lich-table').innerHTML = '<tr><td colspan="7" style="text-align:center;padding:32px;color:#9ca3af">Không có lịch học</td></tr>';
      return;
    }
    
    document.getElementById('dd-lich-table').innerHTML = lich.map(l => `
      <tr>
        <td><strong>#${l.maLichHoc}</strong></td>
        <td>${l.maLopHocPhan}</td>
        <td>${fmtDate(l.ngayHoc)}</td>
        <td>${fmtTime(l.gioBatDau)} – ${fmtTime(l.gioKetThuc)}</td>
        <td>${l.maPhong || '—'}</td>
        <td>${fmtTrangThaiLich(l.trangThai)}</td>
        <td>
          <button class="btn btn-primary btn-sm" onclick="selectLichDiemDanh(${l.maLichHoc})">✅ Điểm danh</button>
        </td>
      </tr>`).join('');
  } catch (e) {
    document.getElementById('dd-lich-table').innerHTML = `<tr><td colspan="7" style="text-align:center;padding:20px;color:var(--danger);">❌ ${e.message}</td></tr>`;
  }
}

async function selectLichDiemDanh(maLichHoc) {
  // Chuyển sang tab chi tiết
  document.querySelectorAll('#page-diemdanh .tab-btn').forEach(b => b.classList.remove('active'));
  document.querySelectorAll('#page-diemdanh .tab-content').forEach(c => c.classList.remove('active'));
  document.querySelector('[data-tab="tab-diemdanh-chitiet"]').classList.add('active');
  document.getElementById('tab-diemdanh-chitiet').classList.add('active');
  document.getElementById('dd-manual-lichid').value = maLichHoc;
  await loadDiemDanhChiTiet(maLichHoc);
}

async function loadDiemDanhChiTiet(overrideId) {
  const idStr = overrideId || document.getElementById('dd-manual-lichid').value;
  const maLichHoc = parseInt(idStr);
  if (!maLichHoc) { showToast('Vui lòng nhập mã lịch học hợp lệ', 'warning'); return; }
  
  currentLichHocId = maLichHoc;
  document.getElementById('dd-chitiet-title').textContent = `✅ Điểm danh buổi học #${maLichHoc}`;
  document.getElementById('dd-sv-list').innerHTML = '<div class="loading"><div class="spinner"></div></div>';
  document.getElementById('dd-actions').style.display = 'none';
  
  try {
    // Lấy danh sách sinh viên theo lịch học
    const data = await apiND(`/api/nguoidung/diemdanh/lichhoc/${maLichHoc}/danhsach-sinhvien`);
    diemDanhSVList = data?.data || data || [];
    
    if (!diemDanhSVList.length) {
      document.getElementById('dd-sv-list').innerHTML = '<div class="empty-state"><div class="empty-icon">👨‍🎓</div><h3>Không có sinh viên</h3><p>Lớp học phần chưa có sinh viên đăng ký</p></div>';
      return;
    }
    
    renderDiemDanhSVList(diemDanhSVList, maLichHoc);
    document.getElementById('dd-actions').style.display = '';
  } catch (e) {
    document.getElementById('dd-sv-list').innerHTML = `<div class="info-box danger">❌ ${e.message}</div>`;
  }
}

function renderDiemDanhSVList(list, maLichHoc) {
  const html = `
    <div style="margin-bottom:12px;display:flex;gap:8px;align-items:center;flex-wrap:wrap;">
      <span style="font-size:13px;color:#64748b;">Đánh dấu nhanh:</span>
      <button class="btn btn-sm" style="background:#dcfce7;color:#15803d;" onclick="markAll('CoMat')">✅ Tất cả có mặt</button>
      <button class="btn btn-sm" style="background:#fef2f2;color:#dc2626;" onclick="markAll('Vang')">❌ Tất cả vắng</button>
    </div>
    <div class="table-container">
      <table>
        <thead><tr><th>#</th><th>Mã SV</th><th>Họ tên</th><th>Điểm danh</th><th>Lý do</th><th>Thao tác</th></tr></thead>
        <tbody>
          ${list.map((sv, i) => {
            const curStatus = sv.trangThai || 'CoMat';
            return `<tr id="dd-row-${sv.maSV}">
              <td>${i+1}</td>
              <td><strong>${sv.maSV}</strong></td>
              <td>${sv.hoTen || sv.maSV}</td>
              <td>
                <div class="attendance-row">
                  <button class="attendance-btn att-present ${curStatus==='CoMat'?'active':''}" onclick="setStatus('${sv.maSV}','CoMat')">✅ Có mặt</button>
                  <button class="attendance-btn att-absent ${curStatus==='Vang'?'active':''}" onclick="setStatus('${sv.maSV}','Vang')">❌ Vắng</button>
                  <button class="attendance-btn att-late ${curStatus==='DiMuon'?'active':''}" onclick="setStatus('${sv.maSV}','DiMuon')">⏰ Muộn</button>
                  <button class="attendance-btn att-excused ${curStatus==='CoPhep'?'active':''}" onclick="setStatus('${sv.maSV}','CoPhep')">📄 Phép</button>
                </div>
              </td>
              <td><input type="text" class="form-control" id="dd-lydo-${sv.maSV}" value="${sv.lyDo||''}" placeholder="Lý do..." style="width:150px;padding:4px 8px;font-size:12px;"></td>
              <td>
                <button class="action-btn edit" onclick="openSuaDiemDanh(${maLichHoc},'${sv.maSV}','${sv.hoTen||sv.maSV}')">✏️ Sửa</button>
              </td>
            </tr>`;
          }).join('')}
        </tbody>
      </table>
    </div>`;
  
  document.getElementById('dd-sv-list').innerHTML = html;
  
  // Lưu trạng thái hiện tại
  diemDanhSVList.forEach(sv => {
    sv._currentStatus = sv.trangThai || 'CoMat';
  });
}

function setStatus(maSV, status) {
  const sv = diemDanhSVList.find(s => s.maSV === maSV);
  if (sv) sv._currentStatus = status;
  
  const row = document.getElementById(`dd-row-${maSV}`);
  if (!row) return;
  row.querySelectorAll('.attendance-btn').forEach(btn => btn.classList.remove('active'));
  const statusMap = { 'CoMat': 'att-present', 'Vang': 'att-absent', 'DiMuon': 'att-late', 'CoPhep': 'att-excused' };
  const targetBtn = row.querySelector(`.${statusMap[status]}`);
  if (targetBtn) targetBtn.classList.add('active');
}

function markAll(status) {
  diemDanhSVList.forEach(sv => setStatus(sv.maSV, status));
}

async function saveAllDiemDanh() {
  if (!currentLichHocId || !diemDanhSVList.length) return;
  
  let successCount = 0, failCount = 0;
  
  for (const sv of diemDanhSVList) {
    const status = sv._currentStatus || 'CoMat';
    const lyDo = document.getElementById(`dd-lydo-${sv.maSV}`)?.value || '';
    
    try {
      await apiND('/api/nguoidung/diemdanh', 'POST', {
        maLichHoc: currentLichHocId,
        maSV: sv.maSV,
        trangThai: status,
        lyDo: lyDo || null,
        nguoiDiemDanh: maGV,
      });
      successCount++;
    } catch {
      // Thử cập nhật nếu đã tồn tại
      try {
        await apiND(`/api/nguoidung/diemdanh/${currentLichHocId}/${sv.maSV}`, 'PUT', {
          trangThai: status,
          lyDo: lyDo || null,
        });
        successCount++;
      } catch { failCount++; }
    }
  }
  
  if (successCount > 0) showToast(`Lưu điểm danh thành công ${successCount} sinh viên!`, 'success');
  if (failCount > 0) showToast(`Lỗi khi lưu ${failCount} sinh viên`, 'error');
}

// ===== XEM TẤT CẢ ĐIỂM DANH =====
async function loadAllDiemDanh() {
  document.getElementById('dd-all-table').innerHTML = '<tr><td colspan="6" style="text-align:center;padding:20px"><div class="spinner" style="margin:auto"></div></td></tr>';
  try {
    const data = await apiND('/api/nguoidung/diemdanh');
    const list = data?.data || data || [];
    
    if (!list.length) {
      document.getElementById('dd-all-table').innerHTML = '<tr><td colspan="6" style="text-align:center;padding:32px;color:#9ca3af">Chưa có bản ghi điểm danh</td></tr>';
      return;
    }
    
    document.getElementById('dd-all-table').innerHTML = list.map(d => `
      <tr>
        <td><strong>#${d.maLichHoc}</strong></td>
        <td>${d.maSV}</td>
        <td>${fmtStatus(d.trangThai)}</td>
        <td>${d.lyDo || '—'}</td>
        <td>${d.nguoiDiemDanh || '—'}</td>
        <td>
          <button class="action-btn edit" onclick="openSuaDiemDanh(${d.maLichHoc},'${d.maSV}','${d.maSV}','${d.trangThai}','${d.lyDo||''}')">✏️ Sửa</button>
          <button class="action-btn delete" onclick="xoaDiemDanh(${d.maLichHoc},'${d.maSV}')">🗑️</button>
        </td>
      </tr>`).join('');
  } catch (e) {
    document.getElementById('dd-all-table').innerHTML = `<tr><td colspan="6" style="text-align:center;padding:20px;color:var(--danger)">❌ ${e.message}</td></tr>`;
  }
}

function openSuaDiemDanh(maLichHoc, maSV, hoTen, trangThai = 'CoMat', lyDo = '') {
  document.getElementById('sdd-malichhoc').value = maLichHoc;
  document.getElementById('sdd-masv').value = maSV;
  document.getElementById('sdd-hoten').value = hoTen || maSV;
  document.getElementById('sdd-trangthai').value = trangThai;
  document.getElementById('sdd-lydo').value = lyDo;
  openModal('modal-sua-dd');
}

async function saveSuaDiemDanh() {
  const maLichHoc = document.getElementById('sdd-malichhoc').value;
  const maSV = document.getElementById('sdd-masv').value;
  const body = {
    trangThai: document.getElementById('sdd-trangthai').value,
    lyDo: document.getElementById('sdd-lydo').value || null,
  };
  
  try {
    await apiND(`/api/nguoidung/diemdanh/${maLichHoc}/${maSV}`, 'PUT', body);
    showToast('Cập nhật điểm danh thành công!', 'success');
    closeModal('modal-sua-dd');
    loadAllDiemDanh();
    if (currentLichHocId) loadDiemDanhChiTiet(currentLichHocId);
  } catch (e) {
    showToast('Lỗi: ' + e.message, 'error');
  }
}

async function xoaDiemDanh(maLichHoc, maSV) {
  if (!confirmAction(`Xóa điểm danh của SV ${maSV} trong buổi #${maLichHoc}?`)) return;
  try {
    await apiND(`/api/nguoidung/diemdanh/${maLichHoc}/${maSV}`, 'DELETE');
    showToast('Xóa điểm danh thành công!', 'success');
    loadAllDiemDanh();
  } catch (e) {
    showToast('Lỗi: ' + e.message, 'error');
  }
}

// ===== INIT TABS =====
document.querySelectorAll('#page-diemdanh .tab-btn').forEach(btn => {
  btn.addEventListener('click', () => {
    document.querySelectorAll('#page-diemdanh .tab-btn').forEach(b => b.classList.remove('active'));
    document.querySelectorAll('#page-diemdanh .tab-content').forEach(c => c.classList.remove('active'));
    btn.classList.add('active');
    document.getElementById(btn.dataset.tab).classList.add('active');
  });
});

// ===== INIT =====
loadDashboard();
