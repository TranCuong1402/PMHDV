// ===== ADMIN PAGE JS =====
// API: Core :5152, Admin :5210, NguoiDung :5211

// AUTH CHECK
if (!requireAuth('Admin')) { /* redirect handled */ }
renderUserInfo();
initSidebarNav();

// ===== DATA CACHES =====
let allGV = [], allSV = [], allMon = [], allLH = [], allLT = [], allTK = [];
let editMode = { gv: false, sv: false, mon: false, lh: false, lt: false, tk: false };

// ===== PAGE CHANGE EVENTS =====
document.querySelectorAll('.nav-item').forEach(item => {
  item.addEventListener('click', () => {
    const page = item.dataset.page;
    const pageTitle = { 'page-dashboard': 'Dashboard', 'page-taikhoan': 'Quản lý tài khoản', 'page-giangvien': 'Giảng viên', 'page-sinhvien': 'Sinh viên', 'page-monhoc': 'Môn học', 'page-lichhoc': 'Lịch học', 'page-lichthi': 'Lịch thi' };
    document.getElementById('page-title').textContent = pageTitle[page] || '';
    
    if (page === 'page-dashboard') loadDashboard();
    else if (page === 'page-taikhoan') loadTaiKhoan();
    else if (page === 'page-giangvien') loadGV();
    else if (page === 'page-sinhvien') loadSV();
    else if (page === 'page-monhoc') loadMon();
    else if (page === 'page-lichhoc') loadLH();
    else if (page === 'page-lichthi') loadLT();
  });
});

// ===== DASHBOARD =====
async function loadDashboard() {
  try {
    const [gvData, svData, monData, lhData, ltData] = await Promise.allSettled([
      apiAdmin('/api/admin/giangvien'),
      apiAdmin('/api/admin/sinhvien'),
      apiAdmin('/api/admin/monhoc'),
      apiAdmin('/api/admin/lichhoc'),
      apiAdmin('/api/admin/lichthi'),
    ]);
    
    const gv  = gvData.status  === 'fulfilled' ? (gvData.value  || []) : [];
    const sv  = svData.status  === 'fulfilled' ? (svData.value  || []) : [];
    const mon = monData.status === 'fulfilled' ? (monData.value || []) : [];
    const lh  = lhData.status  === 'fulfilled' ? (lhData.value  || []) : [];
    const lt  = ltData.status  === 'fulfilled' ? (ltData.value  || []) : [];
    
    document.getElementById('stat-gv').textContent  = gv.length;
    document.getElementById('stat-sv').textContent  = sv.length;
    document.getElementById('stat-mon').textContent = mon.length;
    document.getElementById('stat-lich').textContent = lh.length;
    document.getElementById('stat-thi').textContent  = lt.length;
    
    const rows = gv.slice(0, 10).map(g => `
      <tr>
        <td><strong>${g.maGiangVien || '—'}</strong></td>
        <td>${g.hoTen || '—'}</td>
        <td>${g.khoa || '—'}</td>
        <td>${g.email || '—'}</td>
        <td>${fmtTrangThai(g.trangThai)}</td>
      </tr>`).join('');
    document.getElementById('dash-gv-table').innerHTML = rows || '<tr><td colspan="5" style="text-align:center;padding:20px;color:#9ca3af">Không có dữ liệu</td></tr>';
  } catch (e) {
    showToast('Lỗi tải dashboard: ' + e.message, 'error');
  }
}

// ===== TÀI KHOẢN =====
async function loadTaiKhoan() {
  document.getElementById('tk-table').innerHTML = '<tr><td colspan="8" style="text-align:center;padding:20px"><div class="spinner" style="margin:auto"></div></td></tr>';
  try {
    // Lấy từ admin sinhvien & giangvien để có IdTaiKhoan, 
    // Tài khoản quản lý qua Core API
    const [gvData, svData] = await Promise.all([
      apiAdmin('/api/admin/giangvien'),
      apiAdmin('/api/admin/sinhvien'),
    ]);
    allGV = gvData || []; allSV = svData || [];
    
    // Hiển thị bảng tổng hợp tài khoản từ GV + SV
    renderTKTable(allGV, allSV);
  } catch (e) {
    showToast('Lỗi tải tài khoản: ' + e.message, 'error');
  }
}

function renderTKTable(gv, sv, keyword = '', roleFilter = '') {
  let rows = '';
  
  // Admin rows placeholder
  rows += `<tr style="background:#faf5ff;">
    <td>—</td><td>admin</td><td>Quản trị viên</td><td>admin@system.vn</td>
    <td><span class="badge" style="background:#f3e8ff;color:#7c3aed">Admin</span></td>
    <td>${fmtTrangThai(true)}</td><td>—</td>
    <td><div class="table-actions"><button class="action-btn edit" onclick="openEditTK({id:0,tenDangNhap:'admin',hoTen:'Quản trị viên',email:'admin@system.vn',vaiTro:'Admin',trangThai:true})">✏️ Sửa</button></div></td>
  </tr>`;
  
  const roleOk = r => !roleFilter || r === roleFilter;
  const kwOk = t => !keyword || t.toLowerCase().includes(keyword.toLowerCase());
  
  gv.filter(g => roleOk('GiangVien') && kwOk(g.hoTen + g.maGiangVien + (g.email||''))).forEach(g => {
    rows += `<tr>
      <td>${g.idTaiKhoan || '—'}</td>
      <td>${g.maGiangVien}</td>
      <td>${g.hoTen}</td>
      <td>${g.email || '—'}</td>
      <td><span class="badge" style="background:#e0f2fe;color:#0369a1">Giảng viên</span></td>
      <td>${fmtTrangThai(g.trangThai)}</td>
      <td>—</td>
      <td><div class="table-actions">
        <button class="action-btn edit" onclick='openEditTKFromGV(${JSON.stringify(g)})'>✏️ Sửa</button>
      </div></td>
    </tr>`;
  });
  
  sv.filter(s => roleOk('SinhVien') && kwOk(s.hoTen + s.maSV + (s.email||''))).forEach(s => {
    rows += `<tr>
      <td>${s.idTaiKhoan || '—'}</td>
      <td>${s.maSV}</td>
      <td>${s.hoTen}</td>
      <td>${s.email || '—'}</td>
      <td><span class="badge" style="background:#dcfce7;color:#15803d">Sinh viên</span></td>
      <td>${fmtTrangThai(s.trangThai)}</td>
      <td>—</td>
      <td><div class="table-actions">
        <button class="action-btn edit" onclick='openEditTKFromSV(${JSON.stringify(s)})'>✏️ Sửa</button>
      </div></td>
    </tr>`;
  });
  
  document.getElementById('tk-table').innerHTML = rows || '<tr><td colspan="8" style="text-align:center;padding:24px;color:#9ca3af">Không có dữ liệu</td></tr>';
}

function searchTaiKhoan(val) {
  renderTKTable(allGV, allSV, val, document.getElementById('tk-filter-role').value);
}
function filterTaiKhoan() {
  renderTKTable(allGV, allSV, document.getElementById('tk-search').value, document.getElementById('tk-filter-role').value);
}

function openEditTKFromGV(g) {
  document.getElementById('modal-tk-title').textContent = `Sửa thông tin - ${g.hoTen}`;
  document.getElementById('tk-id').value = g.maGiangVien;
  document.getElementById('tk-tendangnhap').value = g.maGiangVien;
  document.getElementById('tk-hoten').value = g.hoTen;
  document.getElementById('tk-email').value = g.email || '';
  document.getElementById('tk-vaitro').value = 'GiangVien';
  document.getElementById('tk-trangthai').value = String(g.trangThai);
  document.getElementById('tk-matkhaumoi').value = '';
  editMode.tk = { type: 'gv', data: g };
  openModal('modal-taikhoan');
}

function openEditTKFromSV(s) {
  document.getElementById('modal-tk-title').textContent = `Sửa thông tin - ${s.hoTen}`;
  document.getElementById('tk-id').value = s.maSV;
  document.getElementById('tk-tendangnhap').value = s.maSV;
  document.getElementById('tk-hoten').value = s.hoTen;
  document.getElementById('tk-email').value = s.email || '';
  document.getElementById('tk-vaitro').value = 'SinhVien';
  document.getElementById('tk-trangthai').value = String(s.trangThai);
  document.getElementById('tk-matkhaumoi').value = '';
  editMode.tk = { type: 'sv', data: s };
  openModal('modal-taikhoan');
}

function openEditTK(tk) {
  document.getElementById('modal-tk-title').textContent = 'Sửa tài khoản';
  document.getElementById('tk-id').value = tk.id;
  document.getElementById('tk-tendangnhap').value = tk.tenDangNhap;
  document.getElementById('tk-hoten').value = tk.hoTen;
  document.getElementById('tk-email').value = tk.email || '';
  document.getElementById('tk-vaitro').value = tk.vaiTro;
  document.getElementById('tk-trangthai').value = String(tk.trangThai);
  document.getElementById('tk-matkhaumoi').value = '';
  editMode.tk = { type: 'admin', data: tk };
  openModal('modal-taikhoan');
}

async function saveTaiKhoan() {
  const mode = editMode.tk;
  const hoten = document.getElementById('tk-hoten').value.trim();
  const email = document.getElementById('tk-email').value.trim();
  const trangthai = document.getElementById('tk-trangthai').value === 'true';
  
  try {
    if (mode.type === 'gv') {
      const updated = { ...mode.data, hoTen: hoten, email, trangThai: trangthai };
      await apiAdmin(`/api/admin/giangvien/${mode.data.maGiangVien}`, 'PUT', updated);
    } else if (mode.type === 'sv') {
      const updated = { ...mode.data, hoTen: hoten, email, trangThai: trangthai };
      await apiAdmin(`/api/admin/sinhvien/${mode.data.maSV}`, 'PUT', updated);
    }
    showToast('Cập nhật tài khoản thành công!', 'success');
    closeModal('modal-taikhoan');
    loadTaiKhoan();
  } catch (e) {
    showToast('Lỗi: ' + e.message, 'error');
  }
}

// ===== GIẢNG VIÊN =====
async function loadGV(keyword = '') {
  document.getElementById('gv-table').innerHTML = '<tr><td colspan="7" style="text-align:center;padding:20px"><div class="spinner" style="margin:auto"></div></td></tr>';
  try {
    const url = keyword ? `/api/admin/giangvien?keyword=${encodeURIComponent(keyword)}` : '/api/admin/giangvien';
    allGV = await apiAdmin(url) || [];
    renderGVTable(allGV);
  } catch (e) { showToast('Lỗi tải giảng viên: ' + e.message, 'error'); }
}

function renderGVTable(data) {
  if (!data.length) {
    document.getElementById('gv-table').innerHTML = '<tr><td colspan="7" style="text-align:center;padding:32px;color:#9ca3af">Không có dữ liệu</td></tr>';
    return;
  }
  document.getElementById('gv-table').innerHTML = data.map(g => `
    <tr>
      <td><strong>${g.maGiangVien}</strong></td>
      <td>${g.hoTen}</td>
      <td>${g.khoa || '—'}</td>
      <td>${g.email || '—'}</td>
      <td>${g.sDT || g.sdt || '—'}</td>
      <td>${fmtTrangThai(g.trangThai)}</td>
      <td><div class="table-actions">
        <button class="action-btn edit" onclick='openEditGV(${JSON.stringify(g)})'>✏️ Sửa</button>
        <button class="action-btn delete" onclick="deleteGV('${g.maGiangVien}')">🗑️ Xóa</button>
      </div></td>
    </tr>`).join('');
}

const searchGV = debounce(val => loadGV(val));

function openAddGV() {
  document.getElementById('modal-gv-title').textContent = 'Thêm giảng viên mới';
  document.getElementById('gv-id').value = '';
  document.getElementById('gv-ma').value = '';
  document.getElementById('gv-hoten').value = '';
  document.getElementById('gv-email').value = '';
  document.getElementById('gv-sdt').value = '';
  document.getElementById('gv-khoa').value = '';
  document.getElementById('gv-trangthai').value = 'true';
  document.getElementById('gv-idtk').value = '';
  document.getElementById('gv-ma').disabled = false;
  editMode.gv = false;
  openModal('modal-gv');
}

function openEditGV(g) {
  document.getElementById('modal-gv-title').textContent = 'Sửa thông tin giảng viên';
  document.getElementById('gv-id').value = g.maGiangVien;
  document.getElementById('gv-ma').value = g.maGiangVien;
  document.getElementById('gv-hoten').value = g.hoTen;
  document.getElementById('gv-email').value = g.email || '';
  document.getElementById('gv-sdt').value = g.sDT || g.sdt || '';
  document.getElementById('gv-khoa').value = g.khoa || '';
  document.getElementById('gv-trangthai').value = String(g.trangThai);
  document.getElementById('gv-idtk').value = g.idTaiKhoan || '';
  document.getElementById('gv-ma').disabled = true;
  editMode.gv = g;
  openModal('modal-gv');
}

async function saveGV() {
  const ma = document.getElementById('gv-ma').value.trim();
  const body = {
    maGiangVien: ma,
    hoTen: document.getElementById('gv-hoten').value.trim(),
    email: document.getElementById('gv-email').value.trim() || null,
    sDT: document.getElementById('gv-sdt').value.trim() || null,
    khoa: document.getElementById('gv-khoa').value.trim() || null,
    trangThai: document.getElementById('gv-trangthai').value === 'true',
    idTaiKhoan: parseInt(document.getElementById('gv-idtk').value) || null,
  };
  if (!body.maGiangVien || !body.hoTen) { showToast('Vui lòng nhập mã và họ tên giảng viên', 'warning'); return; }
  
  try {
    if (editMode.gv) {
      await apiAdmin(`/api/admin/giangvien/${ma}`, 'PUT', body);
      showToast('Cập nhật giảng viên thành công!', 'success');
    } else {
      await apiAdmin('/api/admin/giangvien', 'POST', body);
      showToast('Thêm giảng viên thành công!', 'success');
    }
    closeModal('modal-gv');
    loadGV();
  } catch (e) { showToast('Lỗi: ' + e.message, 'error'); }
}

async function deleteGV(ma) {
  if (!confirmAction(`Xóa giảng viên ${ma}?`)) return;
  try {
    await apiAdmin(`/api/admin/giangvien/${ma}`, 'DELETE');
    showToast('Xóa giảng viên thành công!', 'success');
    loadGV();
  } catch (e) { showToast('Lỗi: ' + e.message, 'error'); }
}

// ===== SINH VIÊN =====
async function loadSV(keyword = '') {
  document.getElementById('sv-table').innerHTML = '<tr><td colspan="9" style="text-align:center;padding:20px"><div class="spinner" style="margin:auto"></div></td></tr>';
  try {
    const url = keyword ? `/api/admin/sinhvien?keyword=${encodeURIComponent(keyword)}` : '/api/admin/sinhvien';
    allSV = await apiAdmin(url) || [];
    renderSVTable(allSV);
  } catch (e) { showToast('Lỗi tải sinh viên: ' + e.message, 'error'); }
}

function renderSVTable(data) {
  if (!data.length) {
    document.getElementById('sv-table').innerHTML = '<tr><td colspan="9" style="text-align:center;padding:32px;color:#9ca3af">Không có dữ liệu</td></tr>';
    return;
  }
  document.getElementById('sv-table').innerHTML = data.map(s => `
    <tr>
      <td><strong>${s.maSV}</strong></td>
      <td>${s.hoTen}</td>
      <td>${s.maLop || '—'}</td>
      <td>${fmtDate(s.ngaySinh)}</td>
      <td>${s.gioiTinh || '—'}</td>
      <td>${s.email || '—'}</td>
      <td>${s.sDT || s.sdt || '—'}</td>
      <td>${fmtTrangThai(s.trangThai)}</td>
      <td><div class="table-actions">
        <button class="action-btn edit" onclick='openEditSV(${JSON.stringify(s)})'>✏️ Sửa</button>
        <button class="action-btn delete" onclick="deleteSV('${s.maSV}')">🗑️ Xóa</button>
      </div></td>
    </tr>`).join('');
}

const searchSV = debounce(val => loadSV(val));

function openAddSV() {
  document.getElementById('modal-sv-title').textContent = 'Thêm sinh viên mới';
  ['sv-ma','sv-hoten','sv-email','sv-sdt','sv-diachi','sv-malop'].forEach(id => document.getElementById(id).value = '');
  document.getElementById('sv-ngaysinh').value = '';
  document.getElementById('sv-gioitinh').value = 'Nam';
  document.getElementById('sv-trangthai').value = 'true';
  document.getElementById('sv-idtk').value = '';
  document.getElementById('sv-ma').disabled = false;
  editMode.sv = false;
  openModal('modal-sv');
}

function openEditSV(s) {
  document.getElementById('modal-sv-title').textContent = 'Sửa thông tin sinh viên';
  document.getElementById('sv-ma').value = s.maSV;
  document.getElementById('sv-hoten').value = s.hoTen;
  document.getElementById('sv-ngaysinh').value = s.ngaySinh ? s.ngaySinh.split('T')[0] : '';
  document.getElementById('sv-gioitinh').value = s.gioiTinh || 'Nam';
  document.getElementById('sv-email').value = s.email || '';
  document.getElementById('sv-sdt').value = s.sDT || s.sdt || '';
  document.getElementById('sv-diachi').value = s.diaChi || s.diachi || '';
  document.getElementById('sv-malop').value = s.maLop || '';
  document.getElementById('sv-trangthai').value = String(s.trangThai);
  document.getElementById('sv-idtk').value = s.idTaiKhoan || '';
  document.getElementById('sv-ma').disabled = true;
  editMode.sv = s;
  openModal('modal-sv');
}

async function saveSV() {
  const ma = document.getElementById('sv-ma').value.trim();
  const body = {
    maSV: ma,
    hoTen: document.getElementById('sv-hoten').value.trim(),
    ngaySinh: document.getElementById('sv-ngaysinh').value || null,
    gioiTinh: document.getElementById('sv-gioitinh').value,
    email: document.getElementById('sv-email').value.trim() || null,
    sDT: document.getElementById('sv-sdt').value.trim() || null,
    diaChi: document.getElementById('sv-diachi').value.trim() || null,
    maLop: document.getElementById('sv-malop').value.trim(),
    trangThai: document.getElementById('sv-trangthai').value === 'true',
    idTaiKhoan: parseInt(document.getElementById('sv-idtk').value) || null,
  };
  if (!body.maSV || !body.hoTen || !body.maLop) { showToast('Vui lòng nhập đủ mã SV, họ tên, mã lớp', 'warning'); return; }
  
  try {
    if (editMode.sv) {
      await apiAdmin(`/api/admin/sinhvien/${ma}`, 'PUT', body);
      showToast('Cập nhật sinh viên thành công!', 'success');
    } else {
      await apiAdmin('/api/admin/sinhvien', 'POST', body);
      showToast('Thêm sinh viên thành công!', 'success');
    }
    closeModal('modal-sv');
    loadSV();
  } catch (e) { showToast('Lỗi: ' + e.message, 'error'); }
}

async function deleteSV(ma) {
  if (!confirmAction(`Xóa sinh viên ${ma}?`)) return;
  try {
    await apiAdmin(`/api/admin/sinhvien/${ma}`, 'DELETE');
    showToast('Xóa sinh viên thành công!', 'success');
    loadSV();
  } catch (e) { showToast('Lỗi: ' + e.message, 'error'); }
}

// ===== MÔN HỌC =====
async function loadMon(keyword = '') {
  document.getElementById('mon-table').innerHTML = '<tr><td colspan="6" style="text-align:center;padding:20px"><div class="spinner" style="margin:auto"></div></td></tr>';
  try {
    const url = keyword ? `/api/admin/monhoc?keyword=${encodeURIComponent(keyword)}` : '/api/admin/monhoc';
    allMon = await apiAdmin(url) || [];
    renderMonTable(allMon);
  } catch (e) { showToast('Lỗi tải môn học: ' + e.message, 'error'); }
}

function renderMonTable(data) {
  if (!data.length) { document.getElementById('mon-table').innerHTML = '<tr><td colspan="6" style="text-align:center;padding:32px;color:#9ca3af">Không có dữ liệu</td></tr>'; return; }
  document.getElementById('mon-table').innerHTML = data.map(m => `
    <tr>
      <td><strong>${m.maMonHoc}</strong></td>
      <td>${m.tenMonHoc}</td>
      <td style="text-align:center;"><strong>${m.soTinChi}</strong> TC</td>
      <td>${m.moTa || '—'}</td>
      <td>${fmtTrangThai(m.trangThai)}</td>
      <td><div class="table-actions">
        <button class="action-btn edit" onclick='openEditMon(${JSON.stringify(m)})'>✏️ Sửa</button>
        <button class="action-btn delete" onclick="deleteMon('${m.maMonHoc}')">🗑️ Xóa</button>
      </div></td>
    </tr>`).join('');
}

const searchMon = debounce(val => loadMon(val));

function openAddMon() {
  document.getElementById('modal-mon-title').textContent = 'Thêm môn học mới';
  ['mon-ma','mon-ten','mon-mota'].forEach(id => document.getElementById(id).value = '');
  document.getElementById('mon-sotinchi').value = '3';
  document.getElementById('mon-trangthai').value = 'true';
  document.getElementById('mon-ma').disabled = false;
  editMode.mon = false;
  openModal('modal-mon');
}

function openEditMon(m) {
  document.getElementById('modal-mon-title').textContent = 'Sửa môn học';
  document.getElementById('mon-ma').value = m.maMonHoc;
  document.getElementById('mon-ten').value = m.tenMonHoc;
  document.getElementById('mon-sotinchi').value = m.soTinChi;
  document.getElementById('mon-mota').value = m.moTa || '';
  document.getElementById('mon-trangthai').value = String(m.trangThai);
  document.getElementById('mon-ma').disabled = true;
  editMode.mon = m;
  openModal('modal-mon');
}

async function saveMon() {
  const ma = document.getElementById('mon-ma').value.trim();
  const body = {
    maMonHoc: ma,
    tenMonHoc: document.getElementById('mon-ten').value.trim(),
    soTinChi: parseInt(document.getElementById('mon-sotinchi').value),
    moTa: document.getElementById('mon-mota').value.trim() || null,
    trangThai: document.getElementById('mon-trangthai').value === 'true',
  };
  if (!body.maMonHoc || !body.tenMonHoc || !body.soTinChi) { showToast('Vui lòng nhập đủ thông tin bắt buộc', 'warning'); return; }
  
  try {
    if (editMode.mon) {
      await apiAdmin(`/api/admin/monhoc/${ma}`, 'PUT', body);
      showToast('Cập nhật môn học thành công!', 'success');
    } else {
      await apiAdmin('/api/admin/monhoc', 'POST', body);
      showToast('Thêm môn học thành công!', 'success');
    }
    closeModal('modal-mon');
    loadMon();
  } catch (e) { showToast('Lỗi: ' + e.message, 'error'); }
}

async function deleteMon(ma) {
  if (!confirmAction(`Xóa môn học ${ma}?`)) return;
  try {
    await apiAdmin(`/api/admin/monhoc/${ma}`, 'DELETE');
    showToast('Xóa môn học thành công!', 'success');
    loadMon();
  } catch (e) { showToast('Lỗi: ' + e.message, 'error'); }
}

// ===== LỊCH HỌC =====
async function loadLH(keyword = '') {
  document.getElementById('lh-table').innerHTML = '<tr><td colspan="8" style="text-align:center;padding:20px"><div class="spinner" style="margin:auto"></div></td></tr>';
  try {
    const url = keyword ? `/api/admin/lichhoc?keyword=${encodeURIComponent(keyword)}` : '/api/admin/lichhoc';
    allLH = await apiAdmin(url) || [];
    renderLHTable(allLH);
  } catch (e) { showToast('Lỗi tải lịch học: ' + e.message, 'error'); }
}

function renderLHTable(data) {
  if (!data.length) { document.getElementById('lh-table').innerHTML = '<tr><td colspan="8" style="text-align:center;padding:32px;color:#9ca3af">Không có dữ liệu</td></tr>'; return; }
  document.getElementById('lh-table').innerHTML = data.map(l => `
    <tr>
      <td><strong>${l.maLichHoc}</strong></td>
      <td>${l.maLopHocPhan}</td>
      <td>${fmtDate(l.ngayHoc)}</td>
      <td>${fmtTime(l.gioBatDau)}</td>
      <td>${fmtTime(l.gioKetThuc)}</td>
      <td>${l.maPhong || '—'}</td>
      <td><span class="badge badge-active">${l.trangThai || '—'}</span></td>
      <td><div class="table-actions">
        <button class="action-btn edit" onclick='openEditLH(${JSON.stringify(l)})'>✏️ Sửa</button>
        <button class="action-btn delete" onclick="deleteLH(${l.maLichHoc})">🗑️ Xóa</button>
      </div></td>
    </tr>`).join('');
}

const searchLH = debounce(val => loadLH(val));

function openAddLH() {
  document.getElementById('modal-lh-title').textContent = 'Thêm lịch học mới';
  document.getElementById('lh-id').value = '';
  document.getElementById('lh-malophocphan').value = '';
  document.getElementById('lh-ngayhoc').value = '';
  document.getElementById('lh-giobatdau').value = '';
  document.getElementById('lh-gioketthu').value = '';
  document.getElementById('lh-maphong').value = '';
  document.getElementById('lh-trangthai').value = 'ChuaBatDau';
  editMode.lh = false;
  openModal('modal-lh');
}

function openEditLH(l) {
  document.getElementById('modal-lh-title').textContent = 'Sửa lịch học';
  document.getElementById('lh-id').value = l.maLichHoc;
  document.getElementById('lh-malophocphan').value = l.maLopHocPhan;
  document.getElementById('lh-ngayhoc').value = l.ngayHoc ? l.ngayHoc.split('T')[0] : '';
  document.getElementById('lh-giobatdau').value = fmtTime(l.gioBatDau);
  document.getElementById('lh-gioketthu').value = fmtTime(l.gioKetThuc);
  document.getElementById('lh-maphong').value = l.maPhong || '';
  document.getElementById('lh-trangthai').value = l.trangThai || 'ChuaBatDau';
  editMode.lh = l;
  openModal('modal-lh');
}

async function saveLH() {
  const id = document.getElementById('lh-id').value;
  const body = {
    maLopHocPhan: document.getElementById('lh-malophocphan').value.trim(),
    ngayHoc: document.getElementById('lh-ngayhoc').value,
    gioBatDau: document.getElementById('lh-giobatdau').value + ':00',
    gioKetThuc: document.getElementById('lh-gioketthu').value + ':00',
    maPhong: document.getElementById('lh-maphong').value.trim() || null,
    trangThai: document.getElementById('lh-trangthai').value,
  };
  if (!body.maLopHocPhan || !body.ngayHoc || !body.gioBatDau || !body.gioKetThuc) {
    showToast('Vui lòng nhập đủ thông tin bắt buộc', 'warning'); return;
  }
  
  try {
    if (editMode.lh) {
      await apiAdmin(`/api/admin/lichhoc/${id}`, 'PUT', { ...body, maLichHoc: parseInt(id) });
      showToast('Cập nhật lịch học thành công!', 'success');
    } else {
      await apiAdmin('/api/admin/lichhoc', 'POST', body);
      showToast('Thêm lịch học thành công!', 'success');
    }
    closeModal('modal-lh');
    loadLH();
  } catch (e) { showToast('Lỗi: ' + e.message, 'error'); }
}

async function deleteLH(id) {
  if (!confirmAction(`Xóa lịch học #${id}?`)) return;
  try {
    await apiAdmin(`/api/admin/lichhoc/${id}`, 'DELETE');
    showToast('Xóa lịch học thành công!', 'success');
    loadLH();
  } catch (e) { showToast('Lỗi: ' + e.message, 'error'); }
}

// ===== LỊCH THI =====
async function loadLT(keyword = '') {
  document.getElementById('lt-table').innerHTML = '<tr><td colspan="9" style="text-align:center;padding:20px"><div class="spinner" style="margin:auto"></div></td></tr>';
  try {
    const url = keyword ? `/api/admin/lichthi?keyword=${encodeURIComponent(keyword)}` : '/api/admin/lichthi';
    allLT = await apiAdmin(url) || [];
    renderLTTable(allLT);
  } catch (e) { showToast('Lỗi tải lịch thi: ' + e.message, 'error'); }
}

function renderLTTable(data) {
  if (!data.length) { document.getElementById('lt-table').innerHTML = '<tr><td colspan="9" style="text-align:center;padding:32px;color:#9ca3af">Không có dữ liệu</td></tr>'; return; }
  document.getElementById('lt-table').innerHTML = data.map(l => `
    <tr>
      <td><strong>${l.maLichThi}</strong></td>
      <td>${l.maLopHocPhan}</td>
      <td>${fmtDate(l.ngayThi)}</td>
      <td>${fmtTime(l.gioBatDau)}</td>
      <td>${fmtTime(l.gioKetThuc)}</td>
      <td>${l.maPhong || '—'}</td>
      <td>${l.hinhThucThi || '—'}</td>
      <td>${fmtTrangThai(l.trangThai)}</td>
      <td><div class="table-actions">
        <button class="action-btn edit" onclick='openEditLT(${JSON.stringify(l)})'>✏️ Sửa</button>
        <button class="action-btn delete" onclick="deleteLT(${l.maLichThi})">🗑️ Xóa</button>
      </div></td>
    </tr>`).join('');
}

const searchLT = debounce(val => loadLT(val));

function openAddLT() {
  document.getElementById('modal-lt-title').textContent = 'Thêm lịch thi mới';
  document.getElementById('lt-id').value = '';
  ['lt-malophocphan','lt-maphong'].forEach(id => document.getElementById(id).value = '');
  document.getElementById('lt-ngaythi').value = '';
  document.getElementById('lt-giobatdau').value = '';
  document.getElementById('lt-gioketthuc').value = '';
  document.getElementById('lt-hinhthucthi').value = 'TuLuan';
  document.getElementById('lt-trangthai').value = 'true';
  editMode.lt = false;
  openModal('modal-lt');
}

function openEditLT(l) {
  document.getElementById('modal-lt-title').textContent = 'Sửa lịch thi';
  document.getElementById('lt-id').value = l.maLichThi;
  document.getElementById('lt-malophocphan').value = l.maLopHocPhan;
  document.getElementById('lt-ngaythi').value = l.ngayThi ? l.ngayThi.split('T')[0] : '';
  document.getElementById('lt-giobatdau').value = fmtTime(l.gioBatDau);
  document.getElementById('lt-gioketthuc').value = fmtTime(l.gioKetThuc);
  document.getElementById('lt-maphong').value = l.maPhong || '';
  document.getElementById('lt-hinhthucthi').value = l.hinhThucThi || 'TuLuan';
  document.getElementById('lt-trangthai').value = String(l.trangThai);
  editMode.lt = l;
  openModal('modal-lt');
}

async function saveLT() {
  const id = document.getElementById('lt-id').value;
  const body = {
    maLopHocPhan: document.getElementById('lt-malophocphan').value.trim(),
    ngayThi: document.getElementById('lt-ngaythi').value,
    gioBatDau: document.getElementById('lt-giobatdau').value + ':00',
    gioKetThuc: document.getElementById('lt-gioketthuc').value + ':00',
    maPhong: document.getElementById('lt-maphong').value.trim() || null,
    hinhThucThi: document.getElementById('lt-hinhthucthi').value,
    trangThai: document.getElementById('lt-trangthai').value === 'true',
  };
  if (!body.maLopHocPhan || !body.ngayThi) { showToast('Vui lòng nhập đủ thông tin bắt buộc', 'warning'); return; }
  
  try {
    if (editMode.lt) {
      await apiAdmin(`/api/admin/lichthi/${id}`, 'PUT', { ...body, maLichThi: parseInt(id) });
      showToast('Cập nhật lịch thi thành công!', 'success');
    } else {
      await apiAdmin('/api/admin/lichthi', 'POST', body);
      showToast('Thêm lịch thi thành công!', 'success');
    }
    closeModal('modal-lt');
    loadLT();
  } catch (e) { showToast('Lỗi: ' + e.message, 'error'); }
}

async function deleteLT(id) {
  if (!confirmAction(`Xóa lịch thi #${id}?`)) return;
  try {
    await apiAdmin(`/api/admin/lichthi/${id}`, 'DELETE');
    showToast('Xóa lịch thi thành công!', 'success');
    loadLT();
  } catch (e) { showToast('Lỗi: ' + e.message, 'error'); }
}

// ===== INIT =====
loadDashboard();
