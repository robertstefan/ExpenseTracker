import { ActionIcon, Group, Table, Title } from '@mantine/core';
import { useDeleteUserMutation, useGetUsersQuery } from '../../state/user/api';
import React from 'react';
import { useNavigate } from 'react-router-dom';
import { IconTrash } from '@tabler/icons-react';
import Swal from 'sweetalert2';

const Users = () => {
	const navigate = useNavigate();
	const { data: users = {}, isLoading: isLoadingUsers } = useGetUsersQuery();
	const [deleteUser, deleteUserResult] = useDeleteUserMutation();

	return (
		<>
			<Group justify='space-between'>
				<Title>Users</Title>
			</Group>
			{isLoadingUsers && <p>Loading...</p>}
			{users.items?.length == 0 ? (
				<p>No users yet...</p>
			) : (
				<Table striped highlightOnHover>
					<Table.Thead>
						<Table.Tr>
							<Table.Th>Id</Table.Th>
							<Table.Th>Full Name</Table.Th>
							<Table.Th>Username</Table.Th>
							<Table.Th>Email</Table.Th>
							<Table.Th></Table.Th>
						</Table.Tr>
					</Table.Thead>
					<Table.Tbody>
						{users.items?.map((u) => {
							return (
								<Table.Tr className='cursor-pointer' key={u.id} onClick={() => navigate(u.id)}>
									<Table.Td>{u.id}</Table.Td>
									<Table.Td>
										{u.firstName} {u.lastName}
									</Table.Td>
									<Table.Td>{u.username}</Table.Td>
									<Table.Td>{u.email}</Table.Td>
									<Table.Td>
										<ActionIcon>
											<IconTrash
												onClick={(e) => {
													e.stopPropagation();
													Swal.fire({
														title: 'Are you sure ?',
														text: 'You are about to delete the user ' + u.username,
														icon: 'warning',
													}).then(async (result) => result.isConfirmed && (await deleteUser(u.id)));
												}}
											/>
										</ActionIcon>
									</Table.Td>
								</Table.Tr>
							);
						})}
					</Table.Tbody>
				</Table>
			)}
		</>
	);
};

export default Users;
