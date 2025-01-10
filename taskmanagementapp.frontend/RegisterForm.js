import React, { useState } from 'react';
import axios from 'axios';

const RegisterForm = () => {
    const [user, setUser] = useState({
        name: '',
        identityNumber: '',
        email: '',
        password: '',
        role: 'User'
    });

    const handleChange = (e) => {
        const { name, value } = e.target;
        setUser((prevState) => ({
            ...prevState,
            [name]: value
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const response = await axios.post('http://localhost:5000/api/user/register', user);
            alert('User registered successfully');
            console.log(response.data);
        } catch (error) {
            alert('Error occurred while registering the user');
            console.error(error);
        }
    };

    return (
        <div>
            <h2>Register User</h2>
            <form onSubmit={handleSubmit}>
                <div>
                    <label>Name</label>
                    <input
                        type="text"
                        name="name"
                        value={user.name}
                        onChange={handleChange}
                        required
                    />
                </div>
                <div>
                    <label>Identity Number</label>
                    <input
                        type="text"
                        name="identityNumber"
                        value={user.identityNumber}
                        onChange={handleChange}
                        required
                    />
                </div>
                <div>
                    <label>Email</label>
                    <input
                        type="email"
                        name="email"
                        value={user.email}
                        onChange={handleChange}
                        required
                    />
                </div>
                <div>
                    <label>Password</label>
                    <input
                        type="password"
                        name="password"
                        value={user.password}
                        onChange={handleChange}
                        required
                    />
                </div>
                <div>
                    <label>Role</label>
                    <select
                        name="role"
                        value={user.role}
                        onChange={handleChange}
                        required
                    >
                        <option value="User">User</option>
                        <option value="Admin">Admin</option>
                    </select>
                </div>
                <button type="submit">Register</button>
            </form>
        </div>
    );
};

export default RegisterForm;
