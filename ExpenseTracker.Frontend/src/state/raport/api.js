import api from '../emptySplitApi';

export const raportEndpoints = api.injectEndpoints({
  endpoints: (builder) => ({
    getRaportSummary: builder.query({
      query: (params) => ({
        url: '/raport/summary',
        method: 'POST',
        params,
      }),
      transformResponse: (res) => res,
      providesTags: (result) => [{ type: 'Raport', id: result?.data?.id }],
    }),
    getTopCategories: builder.query({
      query: (params) => ({
        url: '/raport/categories',
        method: 'POST',
        params,
      }),
      transformResponse: (res) => res,
      providesTags: (result) => [{ type: 'Raport', id: result?.data?.id }],
    }),
  }),
});

export const { useGetRaportSummaryQuery, useGetTopCategoriesQuery } = raportEndpoints;
