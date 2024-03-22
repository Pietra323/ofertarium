import axios from 'axios';

interface LoginResponse {
    Token: string;
}

export async function login(username: string, password: string): Promise<boolean> {
    try {
        const response = await axios.post<LoginResponse>('http://localhost:5004/api/users/login', { username, password });
        const token = response.data.Token;
        localStorage.setItem('jwtToken', token);
        return true;
    } catch (error) {
        console.error('Błąd logowania:', error);
        return false;
    }
}
