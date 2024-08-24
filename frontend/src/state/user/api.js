import api from '../emptySplitApi';

export const userEndpoints = api.injectEndpoints({
	endpoints: (builder) => ({
		getUsers: builder.query({
			query: (params) => ({ url: '/users/list', params }),
		}),
	}),
});

export const { useGetUsersQuery } = userEndpoints;
