export const userEndpoints = api.injectEndpoints({
    endpoints: (builder)=>({
        getUsers: builder.query({
            query: () => ({url: 'users/all'}),
            transformResponse: (res) => res
        })
    })
})

export const {useGetUsersQuery} = userEndpoints