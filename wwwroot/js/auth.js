// Функция для API-входа
async function apiLogin(username, password) {
    try {
        const response = await fetch('/api/account/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                username: username,
                password: password
            })
        });

        if (!response.ok) {
            const error = await response.json();
            throw new Error(error.message || 'Login failed');
        }

        const data = await response.json();
        // Сохраняем токен
        localStorage.setItem('jwtToken', data.Token);
        return data;
    } catch (error) {
        console.error('Login error:', error);
        throw error;
    }
}

// Функция для API-регистрации
async function apiRegister(userData) {
    try {
        const response = await fetch('/api/account/register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(userData)
        });

        if (!response.ok) {
            const error = await response.json();
            throw new Error(error.message || 'Registration failed');
        }

        return await response.json();
    } catch (error) {
        console.error('Registration error:', error);
        throw error;
    }
}