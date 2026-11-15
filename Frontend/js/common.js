// ===== API CONFIGURATION =====
const API = {
  CORE: 'http://localhost:5152',
  ADMIN: 'http://localhost:5210',
  NGUOIDUNG: 'http://localhost:5211',
};

// ===== AUTH HELPERS =====
const Auth = {
  getToken: () => localStorage.getItem('token'),
  getUser: () => JSON.parse(localStorage.getItem('user') || 'null'),
  getRole: () => localStorage.getItem('role'),
  
  setSession: (data) => {
    localStorage.setItem('token', data.token || data.accessToken);
    localStorage.setItem('role', data.vaiTro || data.role);
    localStorage.setItem('user', JSON.stringify(data));
    if (data.refreshToken) localStorage.setItem('refreshToken', data.refreshToken);
  },
  
  clear: () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    localStorage.removeItem('role');
    localStorage.removeItem('refreshToken');
  },
  
  isLoggedIn: () => !!localStorage.getItem('token'),
  
  headers: () => ({
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${Auth.getToken()}`
  })
};

// ===== HTTP HELPERS =====
async function apiFetch(baseUrl, path, method = 'GET', body = null, auth = true) {
  const opts = {
    method,
    headers: auth ? Auth.headers() : { 'Content-Type': 'application/json' }
  };
  if (body) opts.body = JSON.stringify(body);
  
  try {
    const res = await fetch(`${baseUrl}${path}`, opts);
    const data = await res.json().catch(() => ({}));
    if (!res.ok) throw new Error(data.message || `Lỗi ${res.status}`);
    return data;
  } catch (e) {
    if (e.message === 'Failed to fetch') throw new Error('Không thể kết nối đến server. Hãy kiểm tra API đang chạy.');
    throw e;
  }
}

const apiCore    = (path, m, b, a) => apiFetch(API.CORE, path, m, b, a);
const apiAdmin   = (path, m, b)    => apiFetch(API.ADMIN, path, m, b, true);
const apiND      = (path, m, b)    => apiFetch(API.NGUOIDUNG, path, m, b, true);

// ===== TOAST =====
function showToast(msg, type = 'success') {
  const container = document.getElementById('toast-container') || (() => {
    const el = document.createElement('div');
    el.id = 'toast-container';
    el.className = 'toast-container';
    document.body.appendChild(el);
    return el;
  })();
  
  const icons = { success: '✅', error: '❌', warning: '⚠️' };
  const toast = document.createElement('div');
  toast.className = `toast ${type}`;
  toast.innerHTML = `<span>${icons[type]||'ℹ️'}</span><span>${msg}</span>`;
  container.appendChild(toast);
  setTimeout(() => { toast.style.opacity = '0'; toast.style.transform = 'translateX(100%)'; toast.style.transition = '0.3s'; setTimeout(() => toast.remove(), 300); }, 3000);
}

// ===== MODAL HELPERS =====
function openModal(id) { document.getElementById(id).classList.add('show'); }
function closeModal(id) { document.getElementById(id).classList.remove('show'); }
function closeAllModals() { document.querySelectorAll('.modal-overlay').forEach(m => m.classList.remove('show')); }
document.addEventListener('click', e => { if (e.target.classList.contains('modal-overlay')) closeAllModals(); });
document.addEventListener('keydown', e => { if (e.key === 'Escape') closeAllModals(); });

// ===== TABLE HELPERS =====
function renderTable(tbodyId, rows, cols) {
  const tbody = document.getElementById(tbodyId);
  if (!tbody) return;
  if (!rows || rows.length === 0) {
    tbody.innerHTML = `<tr><td colspan="${cols}" style="text-align:center;padding:32px;color:#9ca3af;">Không có dữ liệu</td></tr>`;
    return;
  }
  tbody.innerHTML = rows;
}

// ===== FORMAT HELPERS =====
function fmtDate(d) {
  if (!d) return '—';
  const dt = new Date(d);
  if (isNaN(dt)) return d;
  return dt.toLocaleDateString('vi-VN');
}
function fmtDateTime(d) {
  if (!d) return '—';
  const dt = new Date(d);
  if (isNaN(dt)) return d;
  return dt.toLocaleString('vi-VN');
}
function fmtTime(t) {
  if (!t) return '—';
  if (typeof t === 'string') return t.substring(0, 5);
  return t;
}
function fmtStatus(s) {
  const map = {
    true: '<span class="badge badge-active">Hoạt động</span>',
    false: '<span class="badge badge-inactive">Vô hiệu</span>',
    'CoMat': '<span class="badge badge-present">Có mặt</span>',
    'Vang': '<span class="badge badge-absent">Vắng</span>',
    'DiMuon': '<span class="badge badge-late">Đi muộn</span>',
    'CoPhep': '<span class="badge badge-excused">Có phép</span>',
    'DangCho': '<span class="badge badge-pending">Đang chờ</span>',
    'DaDuyet': '<span class="badge badge-approved">Đã duyệt</span>',
    'TuChoi': '<span class="badge badge-rejected">Từ chối</span>',
  };
  return map[String(s)] || `<span class="badge">${s || '—'}</span>`;
}
function fmtTrangThai(v) { return v ? '<span class="badge badge-active">✓ Hoạt động</span>' : '<span class="badge badge-inactive">✗ Khóa</span>'; }

// ===== CONFIRM =====
function confirmAction(msg) { return confirm(msg || 'Bạn có chắc chắn?'); }

// ===== LOADING STATE =====
function setLoading(id, loading) {
  const el = document.getElementById(id);
  if (!el) return;
  if (loading) {
    el.dataset.originalHtml = el.innerHTML;
    el.innerHTML = '<span class="spinner" style="width:16px;height:16px;border-width:2px;display:inline-block;"></span>';
    el.disabled = true;
  } else {
    el.innerHTML = el.dataset.originalHtml || el.innerHTML;
    el.disabled = false;
  }
}

// ===== NAVIGATE =====
function navigate(role) {
  const pages = { Admin: 'admin.html', GiangVien: 'giangvien.html', SinhVien: 'sinhvien.html' };
  window.location.href = pages[role] || 'index.html';
}

// ===== LOGOUT =====
async function logout() {
  if (!confirmAction('Bạn có muốn đăng xuất không?')) return;
  try { await apiCore('/api/auth/logout', 'POST'); } catch {}
  Auth.clear();
  window.location.href = 'index.html';
}

// ===== CHECK AUTH =====
function requireAuth(expectedRole) {
  if (!Auth.isLoggedIn()) { window.location.href = 'index.html'; return false; }
  const role = Auth.getRole();
  if (expectedRole && role !== expectedRole && role !== 'Admin') {
    showToast('Bạn không có quyền truy cập trang này', 'error');
    setTimeout(() => window.location.href = 'index.html', 1500);
    return false;
  }
  return true;
}

// ===== TAB SWITCHING =====
function initTabs(containerSelector) {
  document.querySelectorAll(containerSelector + ' .tab-btn').forEach(btn => {
    btn.addEventListener('click', () => {
      const tabId = btn.dataset.tab;
      document.querySelectorAll(containerSelector + ' .tab-btn').forEach(b => b.classList.remove('active'));
      document.querySelectorAll(containerSelector + ' .tab-content').forEach(c => c.classList.remove('active'));
      btn.classList.add('active');
      const content = document.getElementById(tabId);
      if (content) content.classList.add('active');
    });
  });
}

// ===== SIDEBAR NAV =====
function initSidebarNav() {
  document.querySelectorAll('.nav-item').forEach(item => {
    item.addEventListener('click', () => {
      const target = item.dataset.page;
      if (!target) return;
      document.querySelectorAll('.nav-item').forEach(i => i.classList.remove('active'));
      document.querySelectorAll('.page').forEach(p => p.classList.remove('active'));
      item.classList.add('active');
      const page = document.getElementById(target);
      if (page) {
        page.classList.add('active');
        const header = document.getElementById('page-title');
        const sub = document.getElementById('page-subtitle');
        if (header) header.textContent = item.querySelector('.nav-label')?.textContent || '';
        if (sub) sub.textContent = item.dataset.subtitle || '';
      }
    });
  });
}

// ===== SHOW PAGE =====
function showPage(pageId, navLabel) {
  document.querySelectorAll('.page').forEach(p => p.classList.remove('active'));
  document.querySelectorAll('.nav-item').forEach(i => i.classList.remove('active'));
  const page = document.getElementById(pageId);
  if (page) page.classList.add('active');
  document.querySelectorAll('.nav-item').forEach(i => {
    if (i.dataset.page === pageId) i.classList.add('active');
  });
  if (navLabel) {
    const h = document.getElementById('page-title');
    if (h) h.textContent = navLabel;
  }
}

// ===== RENDER USER INFO IN SIDEBAR =====
function renderUserInfo() {
  const user = Auth.getUser();
  const role = Auth.getRole();
  if (!user) return;
  
  const nameEl = document.getElementById('sidebar-user-name');
  const roleEl = document.getElementById('sidebar-user-role');
  const avatarEl = document.getElementById('sidebar-user-avatar');
  
  const roleNames = { Admin: 'Quản trị viên', GiangVien: 'Giảng viên', SinhVien: 'Sinh viên' };
  const displayName = user.hoTen || user.tenDangNhap || 'Người dùng';
  
  if (nameEl) nameEl.textContent = displayName;
  if (roleEl) roleEl.textContent = roleNames[role] || role;
  if (avatarEl) avatarEl.textContent = displayName.charAt(0).toUpperCase();
}

// ===== SEARCH DEBOUNCE =====
function debounce(fn, delay = 400) {
  let timer;
  return (...args) => { clearTimeout(timer); timer = setTimeout(() => fn(...args), delay); };
}
