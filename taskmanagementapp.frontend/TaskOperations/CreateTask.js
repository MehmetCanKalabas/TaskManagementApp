import { useState } from "react";
import axios from "axios";

const TaskCreate = () => {
    const [title, setTitle] = useState("");
    const [description, setDescription] = useState("");
    const [isCompleted, setIsCompleted] = useState(false);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");
    const [successMessage, setSuccessMessage] = useState("");

    const token = localStorage.getItem("jwtToken");
    const apiUrl = "http://localhost:5000/api/tasks";

    const handleSubmit = async (e) => {
        e.preventDefault();

        setLoading(true);
        setError("");
        setSuccessMessage("");

        const taskData = {
            title,
            description,
            isCompleted,
            userId: "user_id_goes_here", // Geçerli kullanýcý ID'si // varsayýlan olarak sabit
        };

        try {
            const response = await axios.post(apiUrl, taskData, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });
            setSuccessMessage(`Görev baþarýyla oluþturuldu! Görev ID: ${response.data.id}`);
            setTitle("");
            setDescription("");
            setIsCompleted(false);
        } catch (err) {
            console.error("Hata:", err);
            setError("Görev oluþturulurken bir hata oluþtu!");
        } finally {
            setLoading(false);
        }

    };

    return (
        <div>
            <h2>Yeni Görev Oluþtur</h2>
            {error && <p style={{ color: "red" }}>{error}</p>}
            {successMessage && <p style={{ color: "green" }}>{successMessage}</p>}
            <form onSubmit={handleSubmit}>
                <div>
                    <label htmlFor="title">Baþlýk</label>
                    <input
                        type="text"
                        id="title"
                        value={title}
                        onChange={(e) => setTitle(e.target.value)}
                        required
                    />
                </div>
                <div>
                    <label htmlFor="description">Açýklama</label>
                    <textarea
                        id="description"
                        value={description}
                        onChange={(e) => setDescription(e.target.value)}
                        required
                    />
                </div>
                <div>
                    <label>
                        <input
                            type="checkbox"
                            checked={isCompleted}
                            onChange={() => setIsCompleted(!isCompleted)}
                        />
                        Tamamlandý
                    </label>
                </div>
                <button type="submit" disabled={loading}>
                    {loading ? "Yükleniyor..." : "Görev Oluþtur"}
                </button>
            </form>
        </div>
    );
};

export default TaskCreate;
