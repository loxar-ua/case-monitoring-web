const API_URL = 'http://localhost:5261/api';

const root = document.getElementById('app-root');
const loader = document.getElementById('loader');
const errorDisplay = document.getElementById('error-message');
const categoryNav = document.getElementById('category-list');

function showError(message) {
    errorDisplay.textContent = message;
}

function setLoader(visible) {
    loader.classList.toggle('hidden', !visible);
}

async function fetchCategories() {
    try {
        const response = await fetch(`${API_URL}/Category`);
        const categories = await response.json();

        categories.forEach((cat) => {
            const btn = document.createElement('button');
            btn.className = 'nav-btn';
            btn.textContent = cat.name;
            btn.addEventListener('click', () => { window.location.hash = `category/${cat.id}`; });
            categoryNav.appendChild(btn);
        });
    } catch (err) {
        showError('Не вдалося завантажити категорії');
    }
}

async function renderList(categoryId = null) {
    setLoader(true);
    root.innerHTML = '';
    try {
        let url = `${API_URL}/Cluster?pageSize=1000`;
        if (categoryId) {
            url += `&categoryId=${categoryId}`;
        }

        const response = await fetch(url);
        const data = await response.json();
        const clusters = data.items || data;

        const listContainer = document.createElement('div');
        listContainer.className = 'cluster-grid';

        if (clusters.length === 0) {
            root.innerHTML = '<p>У цій категорії справ поки немає.</p>';
            return;
        }

        clusters.forEach((item) => {
            const card = document.createElement('div');
            card.className = 'card';
            card.innerHTML = `
                <h3>${item.name}</h3>
                <button class="view-btn" data-id="${item.id}">Детальніше</button>
            `;
            listContainer.appendChild(card);
        });

        root.appendChild(listContainer);

        listContainer.addEventListener('click', (e) => {
            if (e.target.classList.contains('view-btn')) {
                window.location.hash = `cluster/${e.target.dataset.id}`;
            }
        });
    } catch (err) {
        showError('Помилка завантаження списку');
    } finally {
        setLoader(false);
    }
}

async function renderDetails(id) {
    setLoader(true);
    root.innerHTML = '';
    try {
        await fetch(`${API_URL}/Cluster/${id}`, { method: 'PATCH' });

        const response = await fetch(`${API_URL}/Cluster/${id}`);
        const data = await response.json();

        let eventsHtml = '';
        if (data.events && data.events.length > 0) {
            eventsHtml = `
                <div class="events-section">
                    <h2 class="section-title">Хронологія подій</h2>
                    <div class="events-list">
                        ${data.events.map((ev) => {
        const eventDate = ev.date
            ? new Date(ev.date).toLocaleDateString('uk-UA', {
                day: 'numeric',
                month: 'long',
                year: 'numeric',
            })
            : 'Дата невідома';

        return `
                                <div class="event-card">
                                    <hr>
                                    <h4 class="event-title">${ev.title}</h4>
                                    <div class="event-date">${eventDate}</div>
                                    <p class="event-description">
                                        ${ev.description || 'Опис події відсутній'}
                                    </p>
                                </div>
                            `;
    }).join('')}
                    </div>
                </div>
            `;
        }

        root.innerHTML = `
            <div class="cluster-detail">
                <button id="back-btn" class="nav-btn">← Назад до списку</button>
                <h1>${data.name}</h1>
                <p><strong>👁 Переглядів:</strong> ${data.viewCounter}</p>
                <img src="${data.featuredImageURL || ''}" alt="" class="detail-img">
                ${eventsHtml}
            </div>
        `;

        document.getElementById('back-btn').addEventListener('click', () => {
            window.location.hash = '';
        });
    } catch (err) {
        showError('Помилка завантаження даних');
    } finally {
        setLoader(false);
    }
}

function handleRoute() {
    const { hash } = window.location;
    errorDisplay.textContent = '';

    if (hash.startsWith('#cluster/')) {
        const id = hash.split('/')[1];
        renderDetails(id);
    } else if (hash.startsWith('#category/')) {
        const catId = hash.split('/')[1];
        renderList(catId);
    } else {
        renderList();
    }
}

document.getElementById('nav-home').addEventListener('click', () => {
    window.location.hash = '';
});

window.addEventListener('hashchange', handleRoute);
window.addEventListener('DOMContentLoaded', () => {
    fetchCategories();
    handleRoute();
});
