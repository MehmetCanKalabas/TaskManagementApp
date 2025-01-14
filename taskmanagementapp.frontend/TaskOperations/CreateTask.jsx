import { useState } from "react";
import axios from "axios";

const TaskCreate = () => {
    const [title, setTitle] = useState("");
    const [description, setDescription] = useState("");
    const [isCompleted, setIsCompleted] = useState(false);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");
    const [successMessage, setSuccessMessage] = useState("");

    const token = localStorage.getItem("jwtToken"); // JWT token al�n�yor
    const apiUrl = "http://localhost:5000/api/tasks"; // API URL

    // Formu g�nderme i�levi
    const handleSubmit = async (e) => {
        e.preventDefault(); // Formun otomatik g�nderilmesini engeller
        setLoading(true);
        setError(""); // Hata mesaj�n� s�f�rlama
        setSuccessMessage(""); // Ba�ar� mesaj�n� s�f�rlama

        // G�rev verisini haz�rlama
        const taskData = {
            title,
            description,
            isCompleted,
            userId: "user_id_goes_here", // Kullan�c� ID'si dinamik olarak al�nmal�
        };

        try {
            const response = await axios.post(apiUrl, taskData, {
                headers: {
                    Authorization: `Bearer ${token}`, // JWT ile kimlik do�rulama
                },
            });

            setSuccessMessage(`G�rev ba�ar�yla olu�turuldu! G�rev ID: ${response.data.id}`);
            resetForm();
        } catch (err) {
            console.error("Hata:", err);
            setError("G�rev olu�turulurken bir hata olu�tu!");
        } finally {
            setLoading(false);
        }
    };

    // Formu s�f�rlama
    const resetForm = () => {
        setTitle("");
        setDescription("");
        setIsCompleted(false);
    };

    return (
        <div style={{ maxWidth: "500px", margin: "0 auto" }}>
            <h2>Yeni Gorev Olustur</h2>

            {/* Hata ve ba�ar� mesajlar� */}
            {error && <p style={{ color: "red" }}>{error}</p>}
            {successMessage && <p style={{ color: "green" }}>{successMessage}</p>}

            {/* Form */}
            <form onSubmit={handleSubmit}>
                <fieldset style={{ border: "1px solid #ccc", padding: "10px", borderRadius: "5px" }}>
                    <legend>Gorev Bilgileri</legend>

                    {/* Ba�l�k */}
                    <div style={{ marginBottom: "10px" }}>
                        <label htmlFor="title">Baslik</label>
                        <input
                            type="text"
                            id="title"
                            value={title}
                            onChange={(e) => setTitle(e.target.value)}
                            required
                            style={{ display: "block", width: "100%", padding: "5px" }}
                        />
                    </div>

                    {/* A��klama */}
                    <div style={{ marginBottom: "10px" }}>
                        <label htmlFor="description">Aciklama</label>
                        <textarea
                            id="description"
                            value={description}
                            onChange={(e) => setDescription(e.target.value)}
                            required
                            style={{ display: "block", width: "100%", padding: "5px" }}
                        />
                    </div>

                    {/* Tamamland� */}
                    <div style={{ marginBottom: "10px" }}>
                        <label>
                            <input
                                type="checkbox"
                                checked={isCompleted}
                                onChange={() => setIsCompleted(!isCompleted)}
                            />
                            Tamamlandi
                        </label>
                    </div>
                </fieldset>

                {/* G�nderim butonu */}
                <button type="submit" disabled={loading} style={{ marginTop: "10px" }}>
                    {loading ? "Y�kleniyor..." : "Gorev Olustur"}
                </button>
            </form>
        </div>
    );
};

export default TaskCreate;
