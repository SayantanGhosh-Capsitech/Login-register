import React,{useState} from 'react'
import API from "../api/Api";
import { Navigate, useNavigate } from 'react-router-dom';

interface User {
  id: string;
  name: string;
  email: string;
}

const Home = () => {

  const [data, setData] = React.useState<User| null>(null);
  const navigate = useNavigate();

    const response = async()=>{
     try {
      const res = await API.get<User>("/me");
      console.log(res.data); 
      setData(res.data);
    } catch (error) {
      console.error("API error:", error);
      setData(null);
    }
    }
    // const refresh = async()=>{
    //  try {
    //     await API.post('/refresh'); 
    //     // return API(error.config);
    //     console.log("Token refreshed successfully");
    //   } catch (refreshError) {
    //     window.location.href = '/login';
    //     return Promise.reject(refreshError);
    //   }
    // }

    const logout = async () =>{
      try{
        await API.post("/logout");
        navigate("/login")
      }catch(err){
        console.log(err)
      }
    }

  return (
    <div>
      <h1>Home Page</h1>
      <button onClick={response}>Data</button><br/><br/>
      {/* <button onClick={refresh}>Refresh</button> */}
      <button onClick={logout}>Logout</button>
      <p>Name: {data?.name}</p>
      <p>Email: {data?.email}</p>
    </div>
  )
}

export default Home
