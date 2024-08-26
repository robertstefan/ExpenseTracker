import api from '../emptySplitApi';

export const userEndpoints = api.injectEndpoints({
	endpoints: (builder) => ({
		getUsers: builder.query({
			query: () => ({ url: '/users/all' }),
		}),
	}),
});

export const { useGetUsersQuery } = userEndpoints;
