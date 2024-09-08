import api from '../emptySplitApi';

export const userEndpoints = api.injectEndpoints({
	endpoints: (builder) => ({
		getUsers: builder.query({
			query: (params) => ({ url: '/users', params }),
			transformResponse: (res) => res,
			providesTags: (result) =>
				result ? [...result.map(({ id }) => ({ type: 'User', id })), { type: 'User', id: 'LIST' }] : [{ type: 'User', id: 'LIST' }],
		}),
		deleteUser: builder.mutation({
			query: (id) => ({
				url: `/users/${id}`,
				method: 'DELETE',
			}),
			invalidatesTags: [{ type: 'User', id: 'LIST' }],
		}),
		createUser: builder.mutation({
			query: (body) => ({
				url: '/users',
				method: 'POST',
				body,
			}),
			invalidatesTags: [{ type: 'User', id: 'LIST' }],
		}),
	}),
});

export const { useGetUsersQuery, useDeleteUserMutation, useCreateUserMutation } = userEndpoints;
