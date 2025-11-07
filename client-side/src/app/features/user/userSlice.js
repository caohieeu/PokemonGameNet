import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';

const initialState = {
    user: null,
    status: 'idle',
    error: null
}

export const fetchUser = createAsyncThunk('user/fetchUser', async () => {
    const response = await fetch('/api/user');
    return response.json();
})