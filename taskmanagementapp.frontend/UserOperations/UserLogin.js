import { useState } from "react";
import axios from "axios";

const Login = () => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");

    const apiUrl = "https://api-url/api/user/login";

    // Giriþ iþlemi
    const handleLogin = async (e) => {
        e.preventDefault();
        try {
            const response = await axios.post(apiUrl, { email, password });

            // Token'ý localStorage'a kaydediyoruz
            localStorage.setItem("token", response.data.token);

            alert("Giriþ baþarýlý!");
        } catch (err) {
            setError("Geçersiz e-posta veya þifre!");
            console.error("Hata:", err);
        }
    };

    return (
        <div>
            <h2>Giriþ Yap</h2>
            {error && <p>{error}</p>}
            <form onSubmit={handleLogin}>
                <div>
                    <label>Email:</label>
                    <input
                        type="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        required
                    />
                </div>
                <div>
                    <label>Þifre:</label>
                    <input
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
                </div>
                <button type="submit">Giriþ Yap</button>
            </form>
        </div>
    );
};

export default Login;