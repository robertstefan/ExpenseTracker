import api from '../emptySplitApi';

export const categoryEndpoints = api.enhanceEndpoints({ addTagTypes: ['Category'] }).injectEndpoints({
	endpoints: (builder) => ({
		getCategories: builder.query({
			query: (params) => ({ url: '/categories/list', params }),
			transformResponse: (res) => res,
			providesTags: (result) =>
				result
					? [...result.map(({ id }) => ({ type: 'Category', id })), { type: 'Category', id: 'LIST' }]
					: [{ type: 'Category', id: 'LIST' }],
		}),
		getCategory: builder.query({
			query: (id) => ({ url: `/categories/${id}` }),
			transformResponse: (res) => res,
			providesTags: (result) => [{ type: 'Category', id: result?.id }],
		}),
		createCategory: builder.mutation({
			query: (body) => ({
				url: '/categories',
				method: 'POST',
				body,
			}),
			invalidatesTags: [{ type: 'Category', id: 'LIST' }],
		}),
		updateCategory: builder.mutation({
			query: ({ id, ...data }) => ({
				url: `/categories/${id}`,
				method: 'PUT',
				body: data,
			}),
			invalidatesTags: (_result, _error, { id }) => [
				{ type: 'Category', id: 'LIST' },
				{ type: 'Category', id },
			],
		}),
		deleteCategory: builder.mutation({
			query: (id) => ({
				url: `/categories/${id}`,
				method: 'DELETE',
			}),
			invalidatesTags: [{ type: 'Category', id: 'LIST' }],
		}),
	}),
});

export const {
	useGetCategoriesQuery,
	useGetCategoryQuery,
	useUpdateCategoryMutation,
	useCreateCategoryMutation,
	useDeleteCategoryMutation,
} = categoryEndpoints;
