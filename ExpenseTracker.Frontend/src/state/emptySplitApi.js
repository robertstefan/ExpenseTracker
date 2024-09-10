import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

console.log(444, import.meta.env.VITE_API_URL)
const api = createApi({
	baseQuery: fetchBaseQuery({
		baseUrl: import.meta.env.VITE_API_URL,
		credentials : 'include',
		// prepareHeaders: (headers , {getState}) => {
		// 	const token = getState().auth.token
		// 	if(token){
		// 		headers.set("authorization", `Bearer ${token}`)
		// 	}
		// 	return headers
		// }
	}),
	endpoints: () => ({}),
});

export default api;
