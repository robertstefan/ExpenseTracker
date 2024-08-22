import { ActionIcon, Button, Group, Table, Title } from '@mantine/core';
import React from 'react';
import { useDeleteCategoryMutation, useGetCategoriesQuery } from '../../state/category/api';
import { IconTrash } from '@tabler/icons-react';
import { Link, useNavigate } from 'react-router-dom';

export default function Category() {
	const navigate = useNavigate();

	const { data: categories = {}, isLoading: isLoadingCategories } = useGetCategoriesQuery();

	const [deleteCategory, deleteCategoryResult] = useDeleteCategoryMutation();

	return (
		<>
			<Group justify='space-between'>
				<Title>Category</Title>
				<Link to='/category/new'>
					<Button>Add</Button>
				</Link>
			</Group>
			{isLoadingCategories && <p>Loading...</p>}
			<Table striped highlightOnHover>
				<Table.Thead>
					<Table.Tr>
						<Table.Th>Id</Table.Th>
						<Table.Th>Name</Table.Th>
					</Table.Tr>
				</Table.Thead>
				<Table.Tbody>
					{categories.items?.map((c) => {
						return (
							<Table.Tr className='cursor-pointer' key={c.id} onClick={() => navigate('/category/' + c.id)}>
								<Table.Td>{c.id}</Table.Td>
								<Table.Td>{c.name}</Table.Td>
								<Table.Td>
									<ActionIcon>
										<IconTrash onClick={async () => await deleteCategory(c.id)} />
									</ActionIcon>
								</Table.Td>
							</Table.Tr>
						);
					})}
				</Table.Tbody>
			</Table>
		</>
	);
}
