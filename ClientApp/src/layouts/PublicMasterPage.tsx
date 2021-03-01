import React from 'react'

function PublicMasterPage({children}:any) {
    return (
        <div>
        <h1>Public Master Page Header</h1>

           {children} 

        <h1>Public Master Page Footer</h1>

        </div>
    )
}

export default PublicMasterPage
