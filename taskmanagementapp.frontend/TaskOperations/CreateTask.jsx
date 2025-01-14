import { useState } from "react";
import axios from "axios";

const TaskCreate = () => {
    const [title, setTitle] = useState("");
    const [description, setDescription] = useState("");
    const [isCompleted, setIsCompleted] = useState(false);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");
    const [successMessage, setSuccessMessage] = useState("");

    const token = localStorage.getItem("jwtToken"); // JWT token alýnýyor
    const apiUrl = "http://localhost:5000/api/tasks"; // API URL

    // Formu gönderme iþlevi
    const handleSubmit = async (e) => {
        e.preventDefault(); // Formun otomatik gönderilmesini engeller
        setLoading(true);
        setError(""); // Hata mesajýný sýfýrlama
        setSuccessMessage(""); // Baþarý mesajýný sýfýrlama

        // Görev verisini hazýrlama
        const taskData = {
            title,
            description,
            isCompleted,
            userId: "user_id_goes_here", // Kullanýcý ID'si dinamik olarak alýnmalý
        };

        try {
            const response = await axios.post(apiUrl, taskData, {
                headers: {
                    Authorization: `Bearer ${token}`, // JWT ile kimlik doðrulama
                },
            });

            setSuccessMessage(`Görev baþarýyla oluþturuldu! Görev ID: ${response.data.id}`);
            resetForm();
        } catch (err) {
            console.error("Hata:", err);
            setError("Görev oluþturulurken bir hata oluþtu!");
        } finally {
            setLoading(false);
        }
    };

    // Formu sýfýrlama
    const resetForm = () => {
        setTitle("");
        setDescription("");
        setIsCompleted(false);
    };

    return (
        <div style={{ maxWidth: "500px", margin: "0 auto" }}>
            <h2>Yeni Gorev Olustur</h2>

            {/* Hata ve baþarý mesajlarý */}
            {error && <p style={{ color: "red" }}>{error}</p>}
            {successMessage && <p style={{ color: "green" }}>{successMessage}</p>}

            {/* Form */}
            <form onSubmit={handleSubmit}>
                <fieldset style={{ border: "1px solid #ccc", padding: "10px", borderRadius: "5px" }}>
                    <legend>Gorev Bilgileri</legend>

                    {/* Baþlýk */}
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

                    {/* Açýklama */}
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

                    {/* Tamamlandý */}
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

                {/* Gönderim butonu */}
                <button type="submit" disabled={loading} style={{ marginTop: "10px" }}>
                    {loading ? "Yükleniyor..." : "Gorev Olustur"}
                </button>
            </form>
        </div>
    );
};

export default TaskCreate;
